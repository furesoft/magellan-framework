using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;

namespace Magellan.Tests.Helpers.TypeGeneration
{
    public class MethodContributor : ITypeContributor
    {
        private readonly string _name;
        private MulticastDelegate _callback;
        private readonly List<KeyValuePair<Type, object[]>> _attributes = new List<KeyValuePair<Type, object[]>>();
        private bool? _required;
        private bool _wasCalled;
        private MethodAttributes _accessibility;

        public MethodContributor(string name)
        {
            _name = name;
            _accessibility = MethodAttributes.Public;
        }

        public MethodContributor Private()
        {
            _accessibility = MethodAttributes.Private;
            return this;
        }

        public MethodContributor Protected()
        {
            _accessibility = MethodAttributes.Family;
            return this;
        }

        public MethodContributor Returns<TArgA, TArgB, TArgC, TArgD, TReturn>(Func<TArgA, TArgB, TArgC, TArgD, TReturn> callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        public MethodContributor Returns<TArgA, TArgB, TArgC, TReturn>(Func<TArgA, TArgB, TArgC, TReturn> callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        public MethodContributor Returns<TArgA, TArgB, TReturn>(Func<TArgA, TArgB, TReturn> callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        public MethodContributor Returns<TArgA, TReturn>(Func<TArgA, TReturn> callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        public MethodContributor Returns<TReturn>(Func<TReturn> callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        public MethodContributor Returns<TArgA, TArgB, TArgC, TArgD>(Action<TArgA, TArgB, TArgC, TArgD> callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        public MethodContributor Returns<TArgA, TArgB, TArgC>(Action<TArgA, TArgB, TArgC> callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        public MethodContributor Returns<TArgA, TArgB>(Action<TArgA, TArgB> callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        public MethodContributor Returns<TArgA>(Action<TArgA> callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        public MethodContributor Returns(Action callback)
        {
            return Returns((MulticastDelegate)callback);
        }

        private MethodContributor Returns(MulticastDelegate callback)
        {
            _callback = callback;
            return this;
        }

        public MethodContributor MustBeCalled()
        {
            _required = true;
            return this;
        }

        public MethodContributor MustNotBeCalled()
        {
            _required = false;
            return this;
        }

        public MethodContributor Attribute<TAttribute>(params object[] values)
        {
            _attributes.Add(new KeyValuePair<Type, object[]>(typeof(TAttribute), values));
            return this;
        }

        void ITypeContributor.Verify()
        {
            if (_required == true)
            {
                Assert.IsTrue(_wasCalled, "The method '{0}' was not called", _name);
            }

            if (_required == false)
            {
                Assert.IsFalse(_wasCalled, "The method '{0}' should not have been called", _name);
            }
        }

        void ITypeContributor.Contribute(TypeBuilder typeBuilder, FieldBuilder implementationField, RuntimeImplementation delegatedImplementation)
        {
            // Define the method
            var methodBuilder = typeBuilder.DefineMethod(_name, _accessibility | MethodAttributes.HideBySig);
            methodBuilder.SetReturnType(_callback.Method.ReturnType);
            methodBuilder.SetParameters(_callback.Method.GetParameters().Select(x => x.ParameterType).ToArray());

            // Define attributes
            foreach (var attribute in _attributes)
            {
                var types = attribute.Value.Select(x => x.GetType()).ToArray();
                var attributeCtor = attribute.Key.GetConstructor(types);
                methodBuilder.SetCustomAttribute(
                    new CustomAttributeBuilder(attributeCtor, attribute.Value)
                    );
            }

            // Define parameters
            var parameters = new List<ParameterBuilder>();
            foreach (var param in _callback.Method.GetParameters())
            {
                parameters.Add(methodBuilder.DefineParameter(param.Position + 1, ParameterAttributes.None, param.Name));
            }

            // Prepare the implementation
            var bodyWriter = methodBuilder.GetILGenerator();
            bodyWriter.Emit(OpCodes.Ldarg_0);
            bodyWriter.Emit(OpCodes.Ldfld, implementationField);
            bodyWriter.Emit(OpCodes.Ldstr, _name);

            // Build the array of objects
            var arguments = bodyWriter.DeclareLocal(typeof(object[]));
            bodyWriter.Emit(OpCodes.Ldc_I4, parameters.Count);
            bodyWriter.Emit(OpCodes.Newarr, typeof(object));
            bodyWriter.Emit(OpCodes.Stloc, arguments);

            // Set the array
            foreach (var parameter in _callback.Method.GetParameters())
            {
                bodyWriter.Emit(OpCodes.Ldloc, arguments);
                bodyWriter.Emit(OpCodes.Ldc_I4, parameter.Position);
                bodyWriter.Emit(OpCodes.Ldarg, parameter.Position + 1);
                if (parameter.ParameterType.IsValueType)
                {
                    bodyWriter.Emit(OpCodes.Box, parameter.ParameterType);
                }
                bodyWriter.Emit(OpCodes.Stelem_Ref);
            }

            // Call Invoke and return the result
            bodyWriter.Emit(OpCodes.Ldloc, arguments);
            bodyWriter.EmitCall(OpCodes.Callvirt, typeof(IImplementation).GetMethod("Invoke"), new Type[0]);
            bodyWriter.Emit(OpCodes.Unbox_Any, _callback.Method.ReturnType);
            bodyWriter.Emit(OpCodes.Stloc_0);
            bodyWriter.Emit(OpCodes.Ldloc_0);
            bodyWriter.Emit(OpCodes.Ret);

            delegatedImplementation.RegisterCallback(_name, methodBuilder, 
                args =>
                    {
                        _wasCalled = true;
                        try
                        {
                            return _callback.DynamicInvoke(args);
                        }
                        catch (TargetInvocationException ex)
                        {
                            throw ex.InnerException;
                        }
                    }
                );
        }
    }
}

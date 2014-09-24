using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Magellan.Tests.Helpers.TypeGeneration
{
    public class TypeBuilder<TBase> where TBase : class
    {
        private readonly string _typeName;
        private readonly ModuleBuilder _moduleBuilder;
        private readonly List<ITypeContributor> _contributors = new List<ITypeContributor>();
        private TBase _instance;
        private RuntimeImplementation _implementation;
        private static int _assemblyId;

        public TypeBuilder(string typeName) : this(typeName, null)
        {
        }

        public TypeBuilder(string typeName, ModuleBuilder moduleBuilder)
        {
            _typeName = typeName;
            _moduleBuilder = moduleBuilder
                ?? AppDomain.CurrentDomain.DefineDynamicAssembly(
                        new AssemblyName("T" + (++_assemblyId)), 
                        AssemblyBuilderAccess.Run)
                    .DefineDynamicModule("T" + (++_assemblyId));
        }

        public MethodContributor Method(string methodName)
        {
            var result = new MethodContributor(methodName);
            _contributors.Add(result);
            return result;
        }

        public TBase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateInstance();
                }
                return _instance;
            }
        }

        private TBase CreateInstance()
        {
            var typeBuilder = _moduleBuilder.DefineType(_typeName);
            typeBuilder.SetParent(typeof(TBase));
            var implementationField = typeBuilder.DefineField("__implementation", typeof(IImplementation), FieldAttributes.Private);
            var baseConstructor = typeof(TBase).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);

            // Constructor
            var method = typeBuilder.DefineMethod(".ctor", MethodAttributes.Public | MethodAttributes.HideBySig);
            method.SetReturnType(typeof(void));
            method.SetParameters(typeof(IImplementation));
            method.DefineParameter(1, ParameterAttributes.None, "implementation");
            var constructorWriter = method.GetILGenerator();

            // Writing body
            constructorWriter.Emit(OpCodes.Ldarg_0);
            constructorWriter.Emit(OpCodes.Call, baseConstructor);
            constructorWriter.Emit(OpCodes.Nop);
            constructorWriter.Emit(OpCodes.Nop);
            constructorWriter.Emit(OpCodes.Ldarg_0);
            constructorWriter.Emit(OpCodes.Ldarg_1);
            constructorWriter.Emit(OpCodes.Stfld, implementationField);
            constructorWriter.Emit(OpCodes.Nop);
            constructorWriter.Emit(OpCodes.Ret);

            _implementation = new RuntimeImplementation();

            // Other methods
            foreach (var contributor in _contributors)
            {
                contributor.Contribute(typeBuilder, implementationField, _implementation);
            }

            var type = typeBuilder.CreateType();
            return (TBase)Activator.CreateInstance(type, _implementation);
        }

        public void VerifyAll()
        {
            foreach (var contributor in _contributors)
            {
                contributor.Verify();
            }
        }
    }
}

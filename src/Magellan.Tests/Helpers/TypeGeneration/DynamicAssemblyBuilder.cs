using System;
using System.Reflection;
using System.Reflection.Emit;
using Magellan.Framework;

namespace Magellan.Tests.Helpers.TypeGeneration
{
    public class DynamicProject
    {
        private readonly AssemblyBuilder _assemblyBuilder;
        private readonly ModuleBuilder _module;
        private static int _assemblyId;
        
        public DynamicProject()
        {
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("DynamicAssembly" + (++_assemblyId)), AssemblyBuilderAccess.Run);
            _module = _assemblyBuilder.DefineDynamicModule("DynamicAssembly" + (++_assemblyId));
        }

        public TypeBuilder<Controller> DefineController(string fullName)
        {
            return Define<Controller>(fullName);
        }

        public TypeBuilder<TView> DefineView<TView>(string fullName) where TView : class
        {
            return Define<TView>(fullName);
        }

        private TypeBuilder<TBase> Define<TBase>(string fullName) where TBase : class
        {
            var builder = new TypeBuilder<TBase>(fullName, _module);
            builder.Instance.ToString();
            return builder;
        }

        public Assembly Assembly
        {
            get
            {
                return _assemblyBuilder;
            }
        }
    }
}

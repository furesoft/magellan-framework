using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Magellan.Tests.Helpers.TypeGeneration
{
    public class RuntimeImplementation : IImplementation
    {
        private readonly List<Registration> _registrations = new List<Registration>();

        public void RegisterCallback(string methodName, MethodInfo method, Func<object[], object> callback)
        {
            _registrations.Add(new Registration {Callback = callback, Definition = method, Name = methodName});
        }

        public object Invoke(string methodName, object[] arguments)
        {
            var implementation = FindRegistration(methodName, arguments);
            return implementation.Callback(arguments);
        }

        private Registration FindRegistration(string methodName, IEnumerable<object> arguments)
        {
            var implementations = _registrations.Where(x => x.Name == methodName);
            if (implementations.Count() == 0)
            {
                throw new NotImplementedException(methodName);
            }
            if (implementations.Count() == 1)
            {
                return implementations.First();
            }

            return implementations.First(x => HasMatchingSignature(x, arguments.Select(y => y == null ? null : y.GetType())));
        }

        private static bool HasMatchingSignature(Registration candidate, IEnumerable<Type> argumentTypes)
        {
            var candidateParameterTypes = candidate.Definition.GetParameters().Select(x => x.GetType());
            if (candidateParameterTypes.Count() != argumentTypes.Count())
            {
                return false;
            }

            return true;
        }

        private class Registration
        {
            public string Name { get; set; }
            public MethodInfo Definition { get; set; }
            public Func<object[], object> Callback { get; set; }
        }
    }
}

using System.Reflection.Emit;

namespace Magellan.Tests.Helpers.TypeGeneration
{
    public interface ITypeContributor
    {
        void Contribute(TypeBuilder typeBuilder, FieldBuilder implementationField, RuntimeImplementation implementation);
        void Verify();
    }
}

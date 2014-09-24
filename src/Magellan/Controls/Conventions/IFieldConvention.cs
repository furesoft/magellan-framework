namespace Magellan.Controls.Conventions
{
    /// <summary>
    /// Implemented by objects which can take information about a field and infer settings about it.
    /// </summary>
    public interface IFieldConvention
    {
        /// <summary>
        /// Configures the field using all of the information in the <paramref name="fieldInfo"/> parameter.
        /// </summary>
        /// <param name="fieldInfo">The field info.</param>
        void Configure(FieldContext fieldInfo);
    }
}
using System.Windows.Data;

namespace Magellan.Utilities
{
    internal static class BindingExtensions
    {
        public static Binding Clone(this Binding original)
        {
            var originalBinding = (Binding)original;
            var duplicate = new Binding();
            duplicate.BindsDirectlyToSource = originalBinding.BindsDirectlyToSource;
            duplicate.Converter = originalBinding.Converter;
            duplicate.ConverterCulture = originalBinding.ConverterCulture;
            duplicate.ConverterParameter = originalBinding.ConverterParameter;
            duplicate.FallbackValue = originalBinding.FallbackValue;
            duplicate.IsAsync = originalBinding.IsAsync;
            duplicate.Mode = originalBinding.Mode;
            duplicate.Path = originalBinding.Path;
            duplicate.UpdateSourceTrigger = originalBinding.UpdateSourceTrigger;
            duplicate.ValidatesOnDataErrors = true;
            duplicate.ValidatesOnExceptions = true;
            
            foreach (var rule in originalBinding.ValidationRules)
            {
                duplicate.ValidationRules.Add(rule);
            }

            if (originalBinding.RelativeSource != null) duplicate.RelativeSource = originalBinding.RelativeSource;
            else if (originalBinding.ElementName != null) duplicate.ElementName = originalBinding.ElementName;
            else if (originalBinding.Source != null) duplicate.Source = originalBinding.Source;

            return duplicate;
        }
    }
}

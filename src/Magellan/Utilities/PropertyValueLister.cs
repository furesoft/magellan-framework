using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Magellan.Utilities
{
    internal static class PropertyValueLister
    {
        public static IEnumerable<KeyValuePair<string, object>> GetProperties(object target)
        {
            if (target == null) return new KeyValuePair<string, object>[0];
            
            if (target is IDictionary<string, object>)
            {
                return (IDictionary<string, object>) target;
            }

            var properties = target.GetType().GetProperties();
            return 
                from property in properties 
                let name = property.Name 
                let value = property.GetValue(target, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, null, CultureInfo.InvariantCulture) 
                select new KeyValuePair<string, object>(name, value);
        }
    }
}

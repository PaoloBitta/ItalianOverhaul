using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianOverhaul
{
    internal class IoUtils
    {
        public static void SetReadOnlyField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (field == null)
            {
                throw new ArgumentException($"Field {fieldName} not found in {obj.GetType().Name}");
            }

            field.SetValue(obj, value);
        }
    }
}

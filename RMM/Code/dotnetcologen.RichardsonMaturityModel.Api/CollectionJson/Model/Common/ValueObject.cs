using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gach.CollectionJson.Model.Common
{
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        public override int GetHashCode()
        {
            var fields = GetFields();
            const int startValue = 17;
            const int multiplier = 59;

            return fields.Select(field => field.GetValue(this)).Where(value => value != null).Aggregate(startValue, (current, value) => current * multiplier + value.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public virtual bool Equals(T other)
        {
            // Si el parametro es null, retorna false
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            // Optimización para un caso de uso común
            // Si es el mismo objeto, retorna true
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Los ValueObject son iguales si todos sus campos son iguales
            var t = GetType();
            var otherType = other.GetType();

            if (t != otherType)
                return false;

            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                var value1 = field.GetValue(other);
                var value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }
                else if (!value1.Equals(value2))
                    return false;
            }

            return true;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            var t = GetType();
            var fields = new List<FieldInfo>();

            while (t != typeof(object))
            {
                fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                t = t.DeclaringType;
            }

            return fields;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiamHT.NullChecker.Core
{
    public static class NullChecker
    {
        public static NullChecker<T> For<T>(T toValidate)
        {
            return new NullChecker<T>(toValidate);
        }
    }

    public class NullChecker<T>
    {
        private readonly T _toValidate;
        private bool _validateValueTypes;

        private List<PropertyInfo> _propertiesToValidate { get; }

        internal NullChecker(T toValidate)
        {
            _toValidate = toValidate;
            _propertiesToValidate = typeof(T).GetProperties().ToList();
        }

        /// <summary>
        /// Removes the given property from the list of properties to validate against. Meaning that validation will pass if the given property is null
        /// </summary>
        /// <param name="propertyName">The name of the property to allow null values for</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the property referenced by name cannot be found on the type set for validation</exception>
        public NullChecker<T> Ignore(string propertyName)
        {
            var prop = _propertiesToValidate.SingleOrDefault(x => x.Name == propertyName);

            if (propertyName == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyName), $"{propertyName} cannot be found in class {typeof(T).Name}");
            }

            _propertiesToValidate.Remove(prop);

            return this;
        }

        /// <summary>
        /// If called before the validate method, will ensure that a default object will also return false as well as nulls. 
        /// This is useful for checking objects such as int and datetime, that are unable to be null by default
        /// </summary>
        public NullChecker<T> AllowValueTypeValidation()
        {
            _validateValueTypes = true;
            return this;
        }

        /// <summary>
        /// Validates against all pending properties on the object and verifies that they are not null.
        /// </summary>
        /// <returns>True if validation passes, false if not</returns>
        public bool Validate()
        {
            if (!_propertiesToValidate.Any())
            {
                throw new InvalidOperationException($"Object {typeof(T).Name} has no properties that can be validated.");
            }

            foreach (var property in _propertiesToValidate)
            {
                var value = property.GetValue(_toValidate);

                if (value == null || HasDefaultValue(property))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HasDefaultValue(PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            if (!_validateValueTypes || !type.IsValueType)
            {
                return false;
            }

            var defaultObject = Activator.CreateInstance(type);

            var property = propertyInfo.GetValue(_toValidate);

            return property.Equals(defaultObject);
        }
    }
}

using System;
using System.Threading;

namespace KoKo.Property {

    /// <summary>
    /// Extensions to <see cref="StoredProperty{T}"/> for atomic operations based on the <see cref="Interlocked"/> class.
    /// </summary>
    public static class InterlockedExtensions {

        /// <summary>
        /// Increase the property's value by one, and also return the newly increased value, all in one atomic operation.
        /// </summary>
        /// <returns>The new, higher value, which is also the property's new value.</returns>
        public static int Increment(this StoredProperty<int> storedProperty) {
            int newValue = Interlocked.Increment(ref storedProperty.StoredValue);
            storedProperty.OnValueChanged(newValue - 1, newValue);
            return newValue;
        }

        /// <summary>
        /// Increase the property's value by one, and also return the newly increased value, all in one atomic operation.
        /// </summary>
        /// <returns>The new, higher value, which is also the property's new value.</returns>
        public static long Increment(this StoredProperty<long> storedProperty) {
            long newValue = Interlocked.Increment(ref storedProperty.StoredValue);
            storedProperty.OnValueChanged(newValue - 1, newValue);
            return newValue;
        }

        /// <summary>
        /// Decrease the property's value by one, and also return the newly decreased value, all in one atomic operation.
        /// </summary>
        /// <returns>The new, lower value, which is also the property's new value.</returns>
        public static int Decrement(this StoredProperty<int> storedProperty) {
            int newValue = Interlocked.Decrement(ref storedProperty.StoredValue);
            storedProperty.OnValueChanged(newValue + 1, newValue);
            return newValue;
        }

        /// <summary>
        /// Decrease the property's value by one, and also return the newly decreased value, all in one atomic operation.
        /// </summary>
        /// <returns>The new, lower value, which is also the property's new value.</returns>
        public static long Decrement(this StoredProperty<long> storedProperty) {
            long newValue = Interlocked.Decrement(ref storedProperty.StoredValue);
            storedProperty.OnValueChanged(newValue + 1, newValue);
            return newValue;
        }

        /// <summary>
        /// Increase the property's value by a specified amount, and also return the new value, all in one atomic operation.
        /// </summary>
        /// <remarks>There is no Subtract() method, but you can pass a negative number to <c>Add()</c> instead.</remarks>
        /// <returns>The new value, which is also the property's new value.</returns>
        public static int Add(this StoredProperty<int> storedProperty, int value) {
            int newValue = Interlocked.Add(ref storedProperty.StoredValue, value);
            if (value != 0) {
                storedProperty.OnValueChanged(newValue - value, newValue);
            }

            return newValue;
        }

        /// <summary>
        /// Increase the property's value by a specified amount, and also return the new value, all in one atomic operation.
        /// </summary>
        /// <remarks>There is no Subtract() method, but you can pass a negative number to <c>Add()</c> instead.</remarks>
        /// <returns>The new value, which is also the property's new value.</returns>
        public static long Add(this StoredProperty<long> storedProperty, long value) {
            long newValue = Interlocked.Add(ref storedProperty.StoredValue, value);
            if (value != 0) {
                storedProperty.OnValueChanged(newValue - value, newValue);
            }

            return newValue;
        }

        /// <summary>
        /// Set the property to a new value, and also get the old value, in one atomic operation.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValue">The new value to set on the property</param>
        /// <returns>The old value of the property</returns>
        public static int Exchange(this StoredProperty<int> storedProperty, int newValue) {
            int oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue);
            if (oldValue != newValue) {
                storedProperty.OnValueChanged(oldValue, newValue);
            }

            return oldValue;
        }

        /// <summary>
        /// Set the property to a new value, and also get the old value, in one atomic operation.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValue">The new value to set on the property</param>
        /// <returns>The old value of the property</returns>
        public static long Exchange(this StoredProperty<long> storedProperty, long newValue) {
            long oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue);
            if (oldValue != newValue) {
                storedProperty.OnValueChanged(oldValue, newValue);
            }

            return oldValue;
        }

        /// <summary>
        /// Set the property to a new value, and also get the old value, in one atomic operation.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValue">The new value to set on the property</param>
        /// <param name="changeTolerance">How different the old and new value have to be for a change event to be fired</param>
        /// <returns>The old value of the property</returns>
        public static double Exchange(this StoredProperty<double> storedProperty, double newValue, double changeTolerance = 0.0001) {
            double oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue);
            if (Math.Abs(oldValue - newValue) > changeTolerance) {
                storedProperty.OnValueChanged(oldValue, newValue);
            }

            return oldValue;
        }

        /// <summary>
        /// Set the property to a new value, and also get the old value, in one atomic operation.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValue">The new value to set on the property</param>
        /// <returns>The old value of the property</returns>
        public static object Exchange(this StoredProperty<object> storedProperty, object newValue) {
            object oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue);
            if (oldValue != newValue) {
                storedProperty.OnValueChanged(oldValue, newValue);
            }

            return oldValue;
        }

        /// <summary>
        /// Set the property to a new value, and also get the old value, in one atomic operation.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValue">The new value to set on the property</param>
        /// <returns>The old value of the property</returns>
        public static T Exchange<T>(this StoredProperty<T> storedProperty, T newValue) where T: class {
            T oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue);
            if (oldValue != newValue) {
                storedProperty.OnValueChanged(oldValue, newValue);
            }

            return oldValue;
        }

        /// <summary>
        /// Compare the property's value to a comparison value. If they are equal, set the property's value to a new value.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValueToAssign">The property's value may be set to this new value</param>
        /// <param name="onlyExchangeIfPropertyValueEquals">The property's value will be compared to this value</param>
        /// <returns>The old value of the property, whether or not the property value changed</returns>
        public static int CompareExchange(this StoredProperty<int> storedProperty, int newValueToAssign, int onlyExchangeIfPropertyValueEquals) {
            int oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, newValueToAssign, onlyExchangeIfPropertyValueEquals);
            if (oldValue != newValueToAssign) {
                storedProperty.OnValueChanged(oldValue, newValueToAssign);
            }

            return oldValue;
        }

        /// <summary>
        /// Compare the property's value to a comparison value. If they are equal, set the property's value to a new value.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValueToAssign">The property's value may be set to this new value</param>
        /// <param name="onlyExchangeIfPropertyValueEquals">The property's value will be compared to this value</param>
        /// <returns>The old value of the property, whether or not the property value changed</returns>
        public static long CompareExchange(this StoredProperty<long> storedProperty, long newValueToAssign, long onlyExchangeIfPropertyValueEquals) {
            long oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, newValueToAssign, onlyExchangeIfPropertyValueEquals);
            if (oldValue != newValueToAssign) {
                storedProperty.OnValueChanged(oldValue, newValueToAssign);
            }

            return oldValue;
        }

        /// <summary>
        /// Compare the property's value to a comparison value. If they are equal, set the property's value to a new value.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValueToAssign">The property's value may be set to this new value</param>
        /// <param name="onlyExchangeIfPropertyValueEquals">The property's value will be compared to this value</param>
        /// <param name="changeTolerance">How different the old and new value have to be for a change event to be fired</param>
        /// <returns>The old value of the property, whether or not the property value changed</returns>
        public static double CompareExchange(this StoredProperty<double> storedProperty, double newValueToAssign, double onlyExchangeIfPropertyValueEquals, double changeTolerance = 0.0001) {
            double oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, newValueToAssign, onlyExchangeIfPropertyValueEquals);
            if (Math.Abs(oldValue - newValueToAssign) > changeTolerance) {
                storedProperty.OnValueChanged(oldValue, newValueToAssign);
            }

            return oldValue;
        }

        /// <summary>
        /// Compare the property's value to a comparison value. If they are equal, set the property's value to a new value.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValueToAssign">The property's value may be set to this new value</param>
        /// <param name="onlyExchangeIfPropertyValueEquals">The property's value will be compared to this value</param>
        /// <returns>The old value of the property, whether or not the property value changed</returns>
        public static object CompareExchange(this StoredProperty<object> storedProperty, object newValueToAssign, object onlyExchangeIfPropertyValueEquals) {
            object oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, newValueToAssign, onlyExchangeIfPropertyValueEquals);
            if (oldValue != newValueToAssign) {
                storedProperty.OnValueChanged(oldValue, newValueToAssign);
            }

            return oldValue;
        }

        /// <summary>
        /// Compare the property's value to a comparison value. If they are equal, set the property's value to a new value.
        /// </summary>
        /// <param name="storedProperty"></param>
        /// <param name="newValueToAssign">The property's value may be set to this new value</param>
        /// <param name="onlyExchangeIfPropertyValueEquals">The property's value will be compared to this value</param>
        /// <returns>The old value of the property, whether or not the property value changed</returns>
        public static T CompareExchange<T>(this StoredProperty<T> storedProperty, T newValueToAssign, T onlyExchangeIfPropertyValueEquals) where T: class {
            T oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, newValueToAssign, onlyExchangeIfPropertyValueEquals);
            if (oldValue != newValueToAssign) {
                storedProperty.OnValueChanged(oldValue, newValueToAssign);
            }

            return oldValue;
        }

    }

}
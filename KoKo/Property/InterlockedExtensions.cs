using System;
using System.Threading;

namespace KoKo.Property {

    public static class InterlockedExtensions {

        public static int Increment(this StoredProperty<int> storedProperty) {
            int newValue = Interlocked.Increment(ref storedProperty.StoredValue);
            storedProperty.OnValueChanged(newValue - 1, newValue);
            return newValue;
        }

        public static long Increment(this StoredProperty<long> storedProperty) {
            long newValue = Interlocked.Increment(ref storedProperty.StoredValue);
            storedProperty.OnValueChanged(newValue - 1, newValue);
            return newValue;
        }

        public static int Decrement(this StoredProperty<int> storedProperty) {
            int newValue = Interlocked.Decrement(ref storedProperty.StoredValue);
            storedProperty.OnValueChanged(newValue + 1, newValue);
            return newValue;
        }

        public static long Decrement(this StoredProperty<long> storedProperty) {
            long newValue = Interlocked.Decrement(ref storedProperty.StoredValue);
            storedProperty.OnValueChanged(newValue + 1, newValue);
            return newValue;
        }

        public static int Add(this StoredProperty<int> storedProperty, int value) {
            int newValue = Interlocked.Add(ref storedProperty.StoredValue, value);
            if (value != 0) {
                storedProperty.OnValueChanged(newValue - value, newValue);
            }

            return newValue;
        }

        public static long Add(this StoredProperty<long> storedProperty, long value) {
            long newValue = Interlocked.Add(ref storedProperty.StoredValue, value);
            if (value != 0) {
                storedProperty.OnValueChanged(newValue - value, newValue);
            }

            return newValue;
        }

        public static int Exchange(this StoredProperty<int> storedProperty, int newValue) {
            int oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue);
            if (oldValue != newValue) {
                storedProperty.OnValueChanged(oldValue, newValue);
            }

            return oldValue;
        }

        public static long Exchange(this StoredProperty<long> storedProperty, long newValue) {
            long oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue);
            if (oldValue != newValue) {
                storedProperty.OnValueChanged(oldValue, newValue);
            }

            return oldValue;
        }

        public static double Exchange(this StoredProperty<double> storedProperty, double newValue, double changeTolerance = 0.0001) {
            double oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue);
            if (Math.Abs(oldValue - newValue) > changeTolerance) {
                storedProperty.OnValueChanged(oldValue, newValue);
            }

            return oldValue;
        }

        public static object Exchange(this StoredProperty<object> storedProperty, object newValue) {
            object oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue);
            if (oldValue != newValue) {
                storedProperty.OnValueChanged(oldValue, newValue);
            }

            return oldValue;
        }

        public static T Exchange<T>(this StoredProperty<T> storedProperty, T newValue2) where T: class {
            T oldValue = Interlocked.Exchange(ref storedProperty.StoredValue, newValue2);
            if (oldValue != newValue2) {
                storedProperty.OnValueChanged(oldValue, newValue2);
            }

            return oldValue;
        }

        public static int CompareExchange(this StoredProperty<int> storedProperty, int possibleNewValue, int assignIfOldValueEquals) {
            int oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, possibleNewValue, assignIfOldValueEquals);
            if (oldValue != possibleNewValue) {
                storedProperty.OnValueChanged(oldValue, possibleNewValue);
            }

            return oldValue;
        }

        public static long CompareExchange(this StoredProperty<long> storedProperty, long possibleNewValue, long assignIfOldValueEquals) {
            long oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, possibleNewValue, assignIfOldValueEquals);
            if (oldValue != possibleNewValue) {
                storedProperty.OnValueChanged(oldValue, possibleNewValue);
            }

            return oldValue;
        }

        public static double CompareExchange(this StoredProperty<double> storedProperty, double possibleNewValue, double assignIfOldValueEquals, double changeTolerance = 0.0001) {
            double oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, possibleNewValue, assignIfOldValueEquals);
            if (Math.Abs(oldValue - possibleNewValue) > changeTolerance) {
                storedProperty.OnValueChanged(oldValue, possibleNewValue);
            }

            return oldValue;
        }

        public static object CompareExchange(this StoredProperty<object> storedProperty, object possibleNewValue, object assignIfOldValueEquals) {
            object oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, possibleNewValue, assignIfOldValueEquals);
            if (oldValue != possibleNewValue) {
                storedProperty.OnValueChanged(oldValue, possibleNewValue);
            }

            return oldValue;
        }

        public static T CompareExchange<T>(this StoredProperty<T> storedProperty, T possibleNewValue, T assignIfOldValueEquals) where T: class {
            T oldValue = Interlocked.CompareExchange(ref storedProperty.StoredValue, possibleNewValue, assignIfOldValueEquals);
            if (oldValue != possibleNewValue) {
                storedProperty.OnValueChanged(oldValue, possibleNewValue);
            }

            return oldValue;
        }

    }

}
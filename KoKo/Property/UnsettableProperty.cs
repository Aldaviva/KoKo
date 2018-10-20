using System.Collections.Generic;
using System.ComponentModel;
using KoKo.Events;

namespace KoKo.Property {

    /// <summary>
    /// A property object that cannot have its <see cref="Value"/> set directly, and can only change its <see cref="Value"/>
    /// based on some external trigger.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> exposed by this property.</typeparam>
    /// See also <seealso cref="DerivedProperty{T}"/>.
    public abstract class UnsettableProperty<T>: Property<T> {

        private readonly object cachedValueLock = new object();
        private readonly object propertyChanged2Lock = new object();

        protected T CachedValue;

        protected UnsettableProperty(T initialValue) {
            CachedValue = initialValue;
        }

        /// <summary>
        /// A gettable, but not directly settable, property value.
        /// </summary>
        public abstract T Value { get; }

        object Property.Value => Value;

        /// <summary>
        /// Fired whenever the <see cref="Value"/> of this property is updated.
        /// </summary>
        public event KoKoPropertyChangedEventHandler<T> PropertyChanged;

        private event PropertyChangedEventHandler PropertyChanged2;

        protected abstract T ComputeValue();

        protected void OnValueChanged(T oldValue, T newValue) {
            PropertyChanged?.Invoke(this, new KoKoPropertyChangedEventArgs<T>(nameof(Value), oldValue, newValue));
            PropertyChanged2?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add {
                lock (propertyChanged2Lock) {
                    PropertyChanged2 += value;
                }
            }
            remove {
                lock (propertyChanged2Lock) {
                    PropertyChanged2 -= value;
                }
            }
        }

        protected void ComputeValueAndFireChangeEvents(object sender = null, PropertyChangedEventArgs propertyChangedEventArgs = null) {
            T oldValue, newValue = ComputeValue();
            bool didValueChange;

            // Can't use Interlocked.Exchange<T>(T, T) because T isn't guaranteed to be a reference type, it might be an int
            lock (cachedValueLock) {
                oldValue = CachedValue;
                didValueChange = !Equals(oldValue, newValue);
                if (didValueChange) {
                    CachedValue = newValue;
                }
            }

            if (didValueChange) {
                OnValueChanged(oldValue, newValue);
            }
        }

        protected void ListenForDependencyUpdates(IEnumerable<Property> dependencies) {
            foreach (Property dependency in dependencies) {
                dependency.PropertyChanged += ComputeValueAndFireChangeEvents;
            }
        }

    }

}
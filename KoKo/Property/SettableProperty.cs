using System.ComponentModel;
using KoKo.Events;

namespace KoKo.Property {

    /// <summary>
    /// A property object that can have its <see cref="Value"/> set directly.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> exposed by this property.</typeparam>
    /// See also <seealso cref="StoredProperty{T}"/>.
    public abstract class SettableProperty<T>: Property<T> {

        private readonly object propertyChanged2Lock = new object();

        /// <summary>
        /// A gettable and settable property value.
        /// </summary>
        /// <inheritdoc />
        public abstract T Value { get; set; }

        object Property.Value => Value;

        /// <summary>
        /// Fired whenever the <c>Value</c> of this property is updated.
        /// </summary>
        public event KoKoPropertyChangedEventHandler<T> PropertyChanged;

        private event PropertyChangedEventHandler PropertyChanged2;

        internal void OnValueChanged(T oldValue, T newValue) {
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

    }

}
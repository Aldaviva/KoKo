using System.ComponentModel;

namespace KoKo.Property
{
    /// <summary>
    /// A property object that can have its <see cref="Value"/> set directly.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> exposed by this property.</typeparam>
    /// See also <seealso cref="StoredProperty{T}"/>.
    public abstract class SettableProperty<T> : Property<T>
    {
        /// <summary>
        /// A gettable and settable property value.
        /// </summary>
        public abstract T Value { get; set; }

        /// <summary>
        /// Fired whenever the <see cref="Value"/> of this property is updated.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        internal void OnValueChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}
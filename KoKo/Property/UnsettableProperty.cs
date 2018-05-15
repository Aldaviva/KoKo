using System.ComponentModel;

namespace KoKo.Property
{
    /// <summary>
    /// A property object that cannot have its <see cref="Value"/> set directly, and can only change its <see cref="Value"/>
    /// based on some external trigger.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> exposed by this property.</typeparam>
    /// See also <seealso cref="DerivedProperty{T}"/>.
    public abstract class UnsettableProperty<T> : Property<T>
    {
        /// <summary>
        /// A gettable, but not directly settable, property value.
        /// </summary>
        public abstract T Value { get; }

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
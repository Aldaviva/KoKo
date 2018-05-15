namespace KoKo.Property
{
    /// <summary>
    /// A property object that stores a single <see cref="Value"/> in memory. The value is set directly on the instance; it is
    /// not derived from any other sources.
    /// </summary>
    /// <typeparam name="T">The type of the stored property value.</typeparam>
    /// See also <seealso cref="DerivedProperty{T}"/>.
    public class StoredProperty<T> : SettableProperty<T>
    {
        private T _value;

        /// <summary>
        /// Create a new <see cref="StoredProperty{T}"/> with the given starting <see cref="Value"/>.
        /// </summary>
        /// <param name="initialValue">The property will contain this <see cref="Value"/> after being created.</param>
        /// <example>
        /// <code>
        /// var name = new StoredProperty{string}("Ben");
        /// Console.WriteLine($"Hello {name.Value}"); // Hello Ben
        /// </code>
        /// </example>
        public StoredProperty(T initialValue)
        {
            _value = initialValue;
        }

        /// <summary>
        /// Gets or sets the value stored in this property.
        /// When setting, it fires a <see cref="SettableProperty{T}.PropertyChanged"/> event if then new value is different from
        /// the old value.
        /// </summary>
        public override T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    OnValueChanged();
                }
            }
        }
    }
}
namespace KoKo.Property
{
    /// <summary>
    /// A data property object which takes its <see cref="Value"/> from another <see cref="Property{T}"/> with no changes.
    /// If you want a property which does apply a transformation based on another property, use a <seealso cref="DerivedProperty{T}"/> instead.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> exposed by this property and its parent.</typeparam>
    public class PassthroughProperty<T> : UnsettableProperty<T>
    {
        private readonly Property<T> _parentProperty;
        public override T Value => _parentProperty.Value;

        /// <summary>
        /// Create a new property whose value is always the same as the given <paramref name="parentProperty"/>.
        /// When <paramref name="parentProperty"/>'s value changes, this property's <see cref="Value"/> will update automatically.
        /// </summary>
        /// <param name="parentProperty">Another existing property whose value should always be returned by this property.</param>
        public PassthroughProperty(Property<T> parentProperty)
        {
            _parentProperty = parentProperty;
            _parentProperty.PropertyChanged += (sender, args) => OnValueChanged();
        }
    }
}
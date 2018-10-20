using System.ComponentModel;
using KoKo.Events;

namespace KoKo.Property {

    /// <summary>
    /// A data property object which takes its <see cref="Value"/> from another <see cref="Property{T}"/> with no changes.
    /// If you want a property which does apply a transformation based on another property, use a <seealso cref="DerivedProperty{T}"/> instead.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> exposed by this property and its parent.</typeparam>
    public class PassthroughProperty<T>: UnsettableProperty<T> {

        private readonly Property<T> parentProperty;

        /// <summary>
        /// The Value of the parent <see cref="Property{T}"/>.
        /// </summary>
        public override T Value => ComputeValue();

        protected override T ComputeValue() {
            return parentProperty.Value;
        }

        /// <summary>
        /// Create a new property whose value is always the same as the given <paramref name="parentProperty"/>.
        /// When <paramref name="parentProperty"/>'s value changes, this property's <see cref="Value"/> will update automatically.
        /// </summary>
        /// <param name="parentProperty">Another existing property whose value should always be returned by this property.</param>
        public PassthroughProperty(Property<T> parentProperty): base(parentProperty.Value) {
            this.parentProperty = parentProperty;
            this.parentProperty.PropertyChanged += OnParentPropertyChanged;
        }

        private void OnParentPropertyChanged(object sender, KoKoPropertyChangedEventArgs<T> args) {
            OnValueChanged(args.OldValue, args.NewValue);
        }
        

    }

}
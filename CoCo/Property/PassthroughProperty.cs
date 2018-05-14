namespace CoCo.Property
{
    public class PassthroughProperty<T> : UnsettableProperty<T>
    {
        private readonly Property<T> _parentProperty;
        public override T Value => _parentProperty.Value;

        public PassthroughProperty(Property<T> parentProperty)
        {
            _parentProperty = parentProperty;
            _parentProperty.PropertyChanged += (sender, args) => OnValueChanged();
        }
    }
}
namespace CoCo.Property
{
    public class StoredProperty<T> : SettableProperty<T>
    {
        private T _value;

        public StoredProperty(T initialValue)
        {
            _value = initialValue;
        }

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
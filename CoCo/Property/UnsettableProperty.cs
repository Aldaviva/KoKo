using System.ComponentModel;

namespace CoCo.Property
{
    public abstract class UnsettableProperty<T> : Property<T>
    {
        public abstract T Value { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnValueChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}
using System.ComponentModel;

namespace CoCo.Property
{
    public abstract class SettableProperty<T> : Property<T>
    {
        public abstract T Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnValueChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}
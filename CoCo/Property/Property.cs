using System.ComponentModel;

namespace CoCo.Property
{
    // for use when you need a list of value-typed properties
    public interface Property : INotifyPropertyChanged
    {
    }

    public interface Property<out T> : Property
    {
        T Value { get; }
    }
}
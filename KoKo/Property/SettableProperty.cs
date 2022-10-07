namespace KoKo.Property {

    public interface SettableProperty<T>: Property<T> {

        new T Value { get; set; }

    }

}
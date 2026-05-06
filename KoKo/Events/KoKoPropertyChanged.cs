using System.ComponentModel;

namespace KoKo.Events;

public delegate void KoKoPropertyChangedEventHandler<T>(object sender, KoKoPropertyChangedEventArgs<T> e);

public interface KoKoNotifyPropertyChanged<T>: INotifyPropertyChanged {

    new event KoKoPropertyChangedEventHandler<T> PropertyChanged;

}

public class KoKoPropertyChangedEventArgs<T>(string propertyName, T oldValue, T newValue): PropertyChangedEventArgs(propertyName) {

    public T OldValue { get; } = oldValue;
    public T NewValue { get; } = newValue;

}
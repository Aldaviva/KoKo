using System.ComponentModel;

namespace KoKo.Events {

    public delegate void KoKoPropertyChangedEventHandler<T>(object sender, KoKoPropertyChangedEventArgs<T> e);

    public interface KoKoNotifyPropertyChanged<T>: INotifyPropertyChanged {

        new event KoKoPropertyChangedEventHandler<T> PropertyChanged;

    }

    public class KoKoPropertyChangedEventArgs<T>: PropertyChangedEventArgs {

        public T OldValue { get; }
        public T NewValue { get; }

        public KoKoPropertyChangedEventArgs(string propertyName, T oldValue, T newValue): base(propertyName) {
            OldValue = oldValue;
            NewValue = newValue;
        }

    }

}
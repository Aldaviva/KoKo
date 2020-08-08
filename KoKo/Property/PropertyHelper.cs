using System.ComponentModel;
using System.Threading;
using KoKo.Events;

namespace KoKo.Property {

    internal class PropertyHelper<T>: KoKoNotifyPropertyChanged<T> {

        private readonly object propertyChanged2Lock = new object();

        public SynchronizationContext? EventSynchronizationContext { get; set; }

        public event KoKoPropertyChangedEventHandler<T>? PropertyChanged;

        private event PropertyChangedEventHandler? PropertyChanged2;

        internal void OnValueChanged(object sender, T oldValue, T newValue) {
            if (EventSynchronizationContext != null) {
                EventSynchronizationContext.Send(delegate { FirePropertyChangedEvents(); }, null);
            } else {
                FirePropertyChangedEvents();
            }

            void FirePropertyChangedEvents() {
                PropertyChanged?.Invoke(sender, new KoKoPropertyChangedEventArgs<T>("Value", oldValue, newValue));
                PropertyChanged2?.Invoke(sender, new PropertyChangedEventArgs("Value"));
            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add {
                lock (propertyChanged2Lock) {
                    PropertyChanged2 += value;
                }
            }
            remove {
                lock (propertyChanged2Lock) {
                    PropertyChanged2 -= value;
                }
            }
        }

    }

}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KoKo.Property {

    public class TentativeProperty<T>: SettableProperty<T>, IDisposable {

        private readonly Property<T> parentProperty;
        private readonly TimeSpan tentativeDuration;
        private readonly object locker = new object();
        private T effectiveValue;
        private CancellationTokenSource cancellationTokenSource;

        public override T Value {
            get => effectiveValue;
            set {
                CancellationToken cancellationToken;
                lock (locker) {
                    cancellationTokenSource?.Cancel();
                    cancellationTokenSource?.Dispose();
                    cancellationTokenSource = new CancellationTokenSource();
                    cancellationToken = cancellationTokenSource.Token;
                }

                StoreValueAndFireChangeEvents(value);

                Task.Delay(tentativeDuration, cancellationToken)
                    .ContinueWith(task => {
                        StoreValueAndFireChangeEvents(parentProperty.Value);
                        lock (locker) {
                            cancellationTokenSource?.Dispose();
                            cancellationTokenSource = null;
                        }
                    }, cancellationToken);
            }
        }

        public TentativeProperty(Property<T> parentProperty, TimeSpan tentativeDuration) {
            this.parentProperty = parentProperty;
            this.tentativeDuration = tentativeDuration;
            effectiveValue = parentProperty.Value;

            parentProperty.PropertyChanged += delegate {
                if (cancellationTokenSource == null) {
                    StoreValueAndFireChangeEvents(parentProperty.Value);
                }
            };
        }

        private void StoreValueAndFireChangeEvents(T newValue) {
            T oldValue;
            bool didValueChange;

            lock (locker) {
                oldValue = effectiveValue;
                didValueChange = !Equals(oldValue, newValue);
                if (didValueChange) {
                    effectiveValue = newValue;
                }
            }

            if (didValueChange) {
                OnValueChanged(oldValue, newValue);
            }
        }

        public void Dispose() {
            lock (locker) {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

    }

}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KoKo.Property {

    /// <summary>
    /// A property that usually passes through the value of another parent property, but setting the value of this property will
    /// temporarily overridden for a specified duration with a custom value. Within this duration, changes to the parent property's
    /// value have no effect on this property's value. Once the duration elapses, this property's value reverts to the parent
    /// property's value. Repeatedly setting this value can prolong the override indefinitely.
    /// </summary>
    /// <typeparam name="T">The type of this property's value, which is the same type as the parent property's value, as well as any
    /// overriding values you may set.</typeparam>
    public class TentativeProperty<T>: SettableProperty<T>, IDisposable {

        private readonly Property<T> parentProperty;
        private readonly TimeSpan tentativeDuration;
        private readonly object locker = new object();
        private T effectiveValue;
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// If you set a value on this property, this object will use your custom value for a specified duration before automatically
        /// reverting to the parent property's value.
        /// </summary>
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

        /// <summary>
        /// Create a property that passes through a parent property's value, but you can also set a custom, temporary value that will
        /// override the parent value for a period of time.
        /// </summary>
        /// <param name="parentProperty">The source property whose value should be passed through to this property</param>
        /// <param name="tentativeDuration">When you set a custom value on this property using <c>Value</c>, how long should it take
        /// for the parent property's value to be reinstated?</param>
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
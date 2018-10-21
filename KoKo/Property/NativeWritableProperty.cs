using System;
using System.ComponentModel;
using System.Reflection;

namespace KoKo.Property {

    public class NativeWritableProperty<T>: SettableProperty<T> {

        private T cachedValue;
        private readonly object cachedValueLock = new object();

        public override T Value {
            get {
                lock (cachedValueLock) {
                    return cachedValue;
                }
            }
            set {
                T oldValue;
                bool didValueChange;

                lock (cachedValueLock) {
                    oldValue = cachedValue;
                    didValueChange = !Equals(oldValue, value);
                    if (didValueChange) {
                        cachedValue = value;
                        nativeProperty.SetValue(nativeObject, value);
                    }
                }

                if (didValueChange) {
                    OnValueChanged(oldValue, value);
                }
            }
        }

        private readonly object nativeObject;
        private readonly string nativePropertyName;
        private readonly PropertyInfo nativeProperty;

        public NativeWritableProperty(INotifyPropertyChanged nativeObject, string nativePropertyName) {
            this.nativeObject = nativeObject;
            this.nativePropertyName = nativePropertyName;
            nativeProperty = nativeObject.GetType().GetTypeInfo().GetDeclaredProperty(nativePropertyName);

            if (!nativeProperty.CanRead) {
                throw new ArgumentException($"The property {nativePropertyName} of object {nativeObject} does not have a visible get accessor.");
            } else if (!nativeProperty.CanWrite) {
                throw new ArgumentException($"The property {nativePropertyName} of object {nativeObject} does not have a visible set accessor.");
            }

            object untypedValue = nativeProperty.GetValue(nativeObject);
            if (untypedValue is T typedValue) {
                cachedValue = typedValue;
            } else {
                throw new ArgumentException($"The property {nativePropertyName} of object {nativeObject} is of type {nativeProperty.PropertyType.Name}, " +
                                            $"but this KoKo {GetType().Name} was constructed with generic type {typeof(T).Name}");
            }

            nativeObject.PropertyChanged += NativePropertyChanged;
        }

        private void NativePropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nativePropertyName) {
                T oldValue, newValue;
                bool didValueChange;

                lock (cachedValueLock) {
                    oldValue = cachedValue;
                    newValue = (T) nativeProperty.GetValue(nativeObject);
                    didValueChange = !Equals(oldValue, newValue);
                    if (didValueChange) {
                        cachedValue = newValue;
                    }
                }

                if (didValueChange) {
                    OnValueChanged(oldValue, newValue);
                }
            }
        }

    }

}
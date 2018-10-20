using System;
using System.ComponentModel;
using System.Reflection;

namespace KoKo.Property {

    public class NativeReadableProperty<T>: UnsettableProperty<T> {

        private readonly object nativeObject;
        private readonly string nativePropertyName;
        private readonly PropertyInfo nativeProperty;

        public NativeReadableProperty(INotifyPropertyChanged nativeObject, string nativePropertyName): base(default) {
            this.nativeObject = nativeObject;
            this.nativePropertyName = nativePropertyName;

            nativeProperty = nativeObject.GetType().GetTypeInfo().GetDeclaredProperty(nativePropertyName);
            if (!nativeProperty.CanRead) {
                throw new ArgumentException($"The property {nativePropertyName} of object {nativeObject} does not have a visible get accessor.");
            }

            object untypedValue = nativeProperty.GetValue(nativeObject);
            if (untypedValue is T typedValue) {
                CachedValue = typedValue;
            } else {
                throw new ArgumentException($"The property {nativePropertyName} of object {nativeObject} is of type {nativeProperty.PropertyType.Name}, " +
                                            $"but this KoKo NativeReadableProperty was constructed with generic type {typeof(T).Name}");
            }

            nativeObject.PropertyChanged += NativePropertyChanged;
        }

        private void NativePropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nativePropertyName) {
                ComputeValueAndFireChangeEvents();
            }
        }

        public override T Value => CachedValue;

        protected override T ComputeValue() {
            return (T) nativeProperty.GetValue(nativeObject);
        }

    }

}
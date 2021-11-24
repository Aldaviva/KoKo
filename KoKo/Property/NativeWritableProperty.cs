using System;
using System.ComponentModel;
using System.Reflection;

namespace KoKo.Property {

    /// <summary>A property that can interoperate with native read-write C# properties that use <c>INotifyPropertyChanged</c>. For readonly C# properties, see <see cref="NativeReadableProperty{T}" />.<br />If you change this object's Value, that change will be propagated to the backing object.</summary>
    /// <typeparam name="T">The type of this property's value, which is also the type of the native C# property</typeparam>
    public class NativeWritableProperty<T>: SettableProperty<T> {

        private readonly object       cachedValueLock = new();
        private readonly object       nativeObject;
        private readonly string       nativePropertyName;
        private readonly PropertyInfo nativeProperty;

        private T cachedValue = default!;

        /// <summary>If you set a new value on this property, that value will be copied to the underlying C# object.</summary>
        public override T Value {
            get {
                lock (cachedValueLock) {
                    return cachedValue;
                }
            }
            set {
                T    oldValue;
                bool didValueChange;

                lock (cachedValueLock) {
                    oldValue       = cachedValue;
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

        /// <exception cref="ArgumentException">if the native property does not exist on the given object, or if its getter isn't accessible, or if its value's type does not match <code>&lt;T&gt;</code></exception>
        private NativeWritableProperty(object nativeObject, string nativePropertyName) {
            this.nativeObject       = nativeObject;
            this.nativePropertyName = nativePropertyName;
            nativeProperty = nativeObject.GetType().GetTypeInfo().GetProperty(nativePropertyName)
                ?? throw new ArgumentException($"The property {nativePropertyName} of type {nativeObject.GetType().Name} does not exist.");

            if (!nativeProperty.CanRead) {
                throw new ArgumentException($"The property {nativeObject.GetType()}.{nativePropertyName} does not have a visible get accessor.");
            } else if (!nativeProperty.CanWrite) {
                throw new ArgumentException($"The property {nativeObject.GetType()}.{nativePropertyName} does not have a visible set accessor.");
            }

            object untypedValue = nativeProperty.GetValue(nativeObject);
            if (untypedValue is T typedValue) {
                cachedValue = typedValue;
            } else if (untypedValue is not null) {
                throw new ArgumentException($"The property {nativePropertyName} of object {nativeObject} is of type {nativeProperty.PropertyType.Name}, " +
                    $"but this KoKo {GetType().Name} was constructed with generic type {typeof(T).Name}");
            }
        }

        /// <summary>Create a KoKo property whose value and change events come from a native C# property.</summary>
        /// <param name="nativeObject">A C# object that implements <see cref="INotifyPropertyChanged" /></param>
        /// <param name="nativePropertyName">The name of a regular C# property (not a KoKo property) on the <c>nativeObject</c> that triggers <c>PropertyChanged</c> events on the object.<br />To be more type-safe here, you can use <c>nameof(MyNativeObjectClass.MyNativeProperty)</c> instead of a string <c>"MyNativeProperty"</c>.</param>
        /// <exception cref="ArgumentException">if the native property does not exist on the given object, or if its getter isn't accessible, or if its value's type does not match <code>&lt;T&gt;</code></exception>
        public NativeWritableProperty(INotifyPropertyChanged nativeObject, string nativePropertyName): this((object) nativeObject, nativePropertyName) {
            nativeObject.PropertyChanged += NativePropertyChanged;
        }

        /// <summary>Create a KoKo property whose value and change events come from a native C# property.</summary>
        /// <param name="nativeObject">A C# object that does not implement <see cref="INotifyPropertyChanged" /></param>
        /// <param name="nativePropertyName">The name of a regular C# property (not a KoKo property) on the <c>nativeObject</c> that triggers an event on the object.<br />To be more type-safe here, you can use <c>nameof(MyNativeObjectClass.MyNativeProperty)</c> instead of a string <c>"MyNativeProperty"</c>.</param>
        /// <param name="nativeEventName">The name of the event that is raised on <c>nativeObject</c> when the value of the <c>nativePropertyName</c> property is changed.<br />To be more type-safe here, you can use <c>nameof(MyNativeObjectClass.MyNativePropertyChanged)</c> instead of a string <c>"MyNativePropertyChanged"</c>.<br />If this parameter is omitted or null, it defaults to appending <c>"Changed"</c> to the <c>nativePropertyName</c> parameter, e.g. <c>new NativeWritableProperty&lt;string&gt;(myToolStripItem, nameof(ToolStripItem.Text))</c> will listen for <c>TextChanged</c> events on <c>myToolStripItem</c>.</param>
        /// <exception cref="ArgumentException">if the native property does not exist on the given object, or if its getter isn't accessible, or if its value's type does not match <code>&lt;T&gt;</code></exception>
        public NativeWritableProperty(object nativeObject, string nativePropertyName, string? nativeEventName = null): this(nativeObject, nativePropertyName) {
            nativeEventName ??= nativePropertyName + "Changed";
            NativeEventListener nativeEventListener = new(nativeObject, nativeEventName);
            nativeEventListener.OnEvent += delegate { NativePropertyChanged(); };
        }

        private void NativePropertyChanged(object? sender = null, PropertyChangedEventArgs? e = null) {
            if (e != null && e.PropertyName != nativePropertyName) return;

            T    oldValue, newValue;
            bool didValueChange;

            lock (cachedValueLock) {
                oldValue       = cachedValue;
                newValue       = (T) nativeProperty.GetValue(nativeObject);
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
﻿using System;
using System.ComponentModel;
using System.Reflection;

namespace KoKo.Property {

    /// <summary>A property that can interoperate with native readonly C# properties that use <c>INotifyPropertyChanged</c>. For read-write C# properties, see <see cref="NativeWritableProperty{T}" />.</summary>
    /// <typeparam name="T">The type of this property's value, which is also the type of the native C# property</typeparam>
    public class NativeReadableProperty<T>: UnsettableProperty<T> {

        private readonly object       nativeObject;
        private readonly string       nativePropertyName;
        private readonly PropertyInfo nativeProperty;

        private NativeReadableProperty(object nativeObject, string nativePropertyName): base(default!) {
            this.nativeObject       = nativeObject;
            this.nativePropertyName = nativePropertyName;

            PropertyInfo? _nativeProperty = nativeObject.GetType().GetTypeInfo().GetProperty(nativePropertyName);
            if (_nativeProperty == null) {
                throw new ArgumentException($"The property {nativePropertyName} of type {nativeObject.GetType().Name} does not exist.");
            } else if (!_nativeProperty.CanRead) {
                throw new ArgumentException($"The property {nativePropertyName} of type {nativeObject.GetType().Name} does not have a visible get accessor.");
            }

            nativeProperty = _nativeProperty;
            object untypedValue = _nativeProperty.GetValue(nativeObject);
            if (untypedValue is T typedValue) { //if the value is null, this fails and throws a confusing error message
                CachedValue = typedValue;
            } else if (untypedValue is not null) {
                throw new ArgumentException($"The property {nativePropertyName} of type {nativeObject.GetType().Name} is of type {_nativeProperty.PropertyType.Name}, " +
                    $"but this KoKo {GetType().Name} was constructed with generic type {typeof(T).Name}");
            }
        }

        /// <summary>Create a KoKo property whose value and change events come from a native C# property.</summary>
        /// <param name="nativeObject">A C# object that implements <see cref="INotifyPropertyChanged" /></param>
        /// <param name="nativePropertyName">The name of a regular C# property (not a KoKo property) on the <c>nativeObject</c> that triggers <c>PropertyChanged</c> events on the object.<br />To be more type-safe here, you can use <c>nameof(MyNativeObjectClass.MyNativeProperty)</c> instead of a string <c>"MyNativeProperty"</c>.</param>
        public NativeReadableProperty(INotifyPropertyChanged nativeObject, string nativePropertyName): this((object) nativeObject, nativePropertyName) {
            nativeObject.PropertyChanged += NativePropertyChanged;
        }

        // public NativeReadableProperty(object nativeObject, string nativePropertyName): this(nativeObject, nativePropertyName, nativePropertyName + "Changed") { }

        /// <summary>Create a KoKo property whose value and change events come from a native C# property.</summary>
        /// <param name="nativeObject">A C# object that does not implement <see cref="INotifyPropertyChanged" /></param>
        /// <param name="nativePropertyName">The name of a regular C# property (not a KoKo property) on the <c>nativeObject</c> that triggers an event on the object.<br />To be more type-safe here, you can use <c>nameof(MyNativeObjectClass.MyNativeProperty)</c> instead of a string <c>"MyNativeProperty"</c>.</param>
        /// <param name="nativeEventName">The name of the event that is raised on <c>nativeObject</c> when the value of the <c>nativePropertyName</c> property is changed.<br />To be more type-safe here, you can use <c>nameof(MyNativeObjectClass.MyNativePropertyChanged)</c> instead of a string <c>"MyNativePropertyChanged"</c>.<br />If this parameter is omitted or null, it defaults to appending <c>"Changed"</c> to the <c>nativePropertyName</c> parameter, e.g. <c>new NativeReadableProperty&lt;string&gt;(myToolStripItem, nameof(ToolStripItem.Text))</c> will listen for <c>TextChanged</c> events on <c>myToolStripItem</c>.</param>
        public NativeReadableProperty(object nativeObject, string nativePropertyName, string? nativeEventName = null): this(nativeObject, nativePropertyName) {
            nativeEventName ??= nativePropertyName + "Changed";
            // public NativeReadableProperty(object nativeObject, string nativePropertyName, string nativeEventName): this(nativePropertyName, nativeObject) {
            var nativeEventListener = new NativeEventListener(nativeObject, nativeEventName);
            nativeEventListener.OnEvent += delegate { ComputeValueAndFireChangeEvents(); };
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
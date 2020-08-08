namespace KoKo.Property {

    /// <summary>
    /// A type of property whose value comes from another dependency <c>Property</c>, which is connected after this object's creation.
    /// This allows this <c>ConnectableProperty</c> to be decoupled from its dependency, providing inversion of control. For example,
    /// you can create a button class which exposes a <c>ConnectableProperty</c> to receive the label text from a property specified by
    /// the code that constructs the button, without the button class itself being aware of where the label came from. An instance
    /// can be connected and disconnected multiple times.
    /// </summary>
    /// <typeparam name="T">The type of this property's value, which is also the type of the values of any properties you can connect
    /// to this</typeparam>
    public class ConnectableProperty<T>: UnsettableProperty<T> {

        private Property<T>? connectedProperty;
        private readonly T disconnectedValue;

        /// <summary>
        /// The value of the currently-connected source property. If no source is currently connected to this property, the
        /// disconnected value will be returned instead.
        /// </summary>
        public override T Value => connectedProperty != null ? connectedProperty.Value : disconnectedValue;

        /// <summary>
        /// Create a new property that is not connected to an dependency property. To connect a dependency property, call
        /// <c>Connect()</c>.
        /// </summary>
        /// <param name="disconnectedValue">The value to return for this property when it is first created and after it has been
        /// disconnected. When this property is connected, this value is not used.</param>
        public ConnectableProperty(T disconnectedValue = default): base(disconnectedValue) {
            this.disconnectedValue = disconnectedValue;
        }

        protected override T ComputeValue() {
            return Value;
        }

        /// <summary>
        /// Connect this property to a <c>source</c> dependency property. Until it is disconnected, this property will return the
        /// source's value as its own value, and changes to the source value will fire change events from this property. Connecting and
        /// disconnected a source will fire change events, as long as the value actually changes.
        /// To stop depending on the source, call <c>Disconnect()</c> or <c>Connect(null)</c>.
        /// </summary>
        /// <param name="source">The upstream dependency property that this object should get its value from.</param>
        public void Connect(Property<T>? source) {
            if (connectedProperty != null) {
                connectedProperty.PropertyChanged -= ComputeValueAndFireChangeEvents;
            }

            connectedProperty = source;
            ComputeValueAndFireChangeEvents();

            if (connectedProperty != null) {
                ListenForDependencyUpdates(new[] { connectedProperty });
            }
        }

        /// <summary>
        /// Stop using any existing source for this property's value and change events. Idempotent. After calling <c>Disconnect()</c>,
        /// this property's value will come from the disconnected value specified in the constructor. Connecting and disconnected a
        /// source will fire change events, as long as the value actually changes.
        /// </summary>
        public void Disconnect() {
            Connect(null);
        }

    }

}
namespace KoKo.Property {

    public class ConnectableProperty<T>: UnsettableProperty<T> {

        private Property<T> connectedProperty;
        private readonly T disconnectedValue;

        public override T Value => connectedProperty != null ? connectedProperty.Value : disconnectedValue;

        public ConnectableProperty(T disconnectedValue = default): base(disconnectedValue) {
            this.disconnectedValue = disconnectedValue;
        }

        protected override T ComputeValue() {
            return Value;
        }

        public void Connect(Property<T> source) {
            if (connectedProperty != null) {
                connectedProperty.PropertyChanged -= ComputeValueAndFireChangeEvents;
            }

            connectedProperty = source;
            ComputeValueAndFireChangeEvents();

            if (connectedProperty != null) {
                ListenForDependencyUpdates(new[] { connectedProperty });
            }
        }

        public void Disconnect() {
            Connect(null);
        }

    }

}
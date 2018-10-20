using FluentAssertions;
using KoKo.Property;
using Xunit;

namespace Test {

    public class TestConnectableProperty {

        private readonly StoredProperty<int> parent = new StoredProperty<int>(1);

        [Fact]
        public void InitialValue() {
            var connectableProperty = new ConnectableProperty<int>(0);
            connectableProperty.Value.Should().Be(0, "before connecting");
        }

        [Fact]
        public void ConnectingChangesValue() {
            var connectableProperty = new ConnectableProperty<int>(0);

            connectableProperty.Connect(parent);
            connectableProperty.Value.Should().Be(1);
        }

        [Fact]
        public void ConnectingWithDifferentValueFiresChange() {
            bool eventFired = false;
            var connectableProperty = new ConnectableProperty<int>(0);
            connectableProperty.PropertyChanged += delegate { eventFired = true; };

            connectableProperty.Connect(parent);
            eventFired.Should().BeTrue();
        }

        [Fact]
        public void ConnectingWithSameValueDoesNotFireChange() {
            bool eventFired = false;
            var connectableProperty = new ConnectableProperty<int>(0);
            connectableProperty.PropertyChanged += delegate { eventFired = true; };

            connectableProperty.Connect(new StoredProperty<int>(0));
            eventFired.Should().BeFalse();
        }

        [Fact]
        public void ParentValueChangesTriggerValueChangeInConnectableProperty() {
            var connectableProperty = new ConnectableProperty<int>(0);
            connectableProperty.Connect(parent);

            parent.Value = 2;
            connectableProperty.Value.Should().Be(2);
        }

        [Fact]
        public void ParentValueChangesTriggerChangeEventsInConnectableProperty() {
            bool eventFired = false;
            var connectableProperty = new ConnectableProperty<int>(0);
            connectableProperty.Connect(parent);
            connectableProperty.PropertyChanged += delegate { eventFired = true; };
            eventFired.Should().BeFalse("haven't changed the parent property yet");

            parent.Value = 2;
            eventFired.Should().BeTrue("parent event changed");
        }

        [Fact]
        public void DisconnectingResetsValueToDisconnectedValue() {
            var connectableProperty = new ConnectableProperty<int>(0);
            connectableProperty.Connect(parent);
            connectableProperty.Value.Should().Be(1, "before disconnecting");

            connectableProperty.Disconnect();
            connectableProperty.Value.Should().Be(0, "after disconnecting");
        }

        [Fact]
        public void DisconnectingFiresChange() {
            bool eventFired = false;
            var connectableProperty = new ConnectableProperty<int>(0);
            connectableProperty.Connect(parent);
            connectableProperty.PropertyChanged += delegate { eventFired = true; };
            eventFired.Should().BeFalse("have not disconnected yet");

            connectableProperty.Disconnect();
            eventFired.Should().BeTrue("disconnected");
        }

        [Fact]
        public void DisconnectingWithSameValueResetsValueToDisconnectedValue() {
            var connectableProperty = new ConnectableProperty<int>(0);
            connectableProperty.Connect(new StoredProperty<int>(0));
            connectableProperty.Value.Should().Be(0, "before disconnecting");

            connectableProperty.Disconnect();
            connectableProperty.Value.Should().Be(0, "after disconnecting");
        }

        [Fact]
        public void DisconnectingWithSameValueDoesNotFireChange() {
            bool eventFired = false;
            var connectableProperty = new ConnectableProperty<int>(0);
            connectableProperty.Connect(new StoredProperty<int>(0));
            connectableProperty.PropertyChanged += delegate { eventFired = true; };
            eventFired.Should().BeFalse("have not disconnected yet");

            connectableProperty.Disconnect();
            eventFired.Should().BeFalse("disconnected, but the value should not have changed from 0 to 0");
        }

    }

}
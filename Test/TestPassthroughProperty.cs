using System.Threading;
using FluentAssertions;
using KoKo.Events;
using KoKo.Property;
using Xunit;

namespace Test {

    public class TestPassthroughProperty {

        private int onPropertyChangedCallCount = 0;

        [Fact]
        public void InitialValue() {
            var stored = new StoredProperty<int>(1);
            var passthrough = new PassthroughProperty<int>(stored);

            passthrough.Value.Should().Be(1);
        }

        [Fact]
        public void ValueIsUpdated() {
            var stored = new StoredProperty<int>(1);
            var passthrough = new PassthroughProperty<int>(stored);
            stored.Value.Should().Be(1);

            stored.Value = 2;
            passthrough.Value.Should().Be(2);
        }

        [Fact]
        public void EventsAreTriggered() {
            var stored = new StoredProperty<int>(1);
            var passthrough = new PassthroughProperty<int>(stored);
            stored.Value.Should().Be(1);

            int eventsTriggeredCount = 0;
            passthrough.PropertyChanged += (sender, args) => {
                eventsTriggeredCount++;
                passthrough.Value.Should().Be(2);
            };
            passthrough.PropertyChanged += OnPropertyChanged;

            stored.Value = 2;

            eventsTriggeredCount.Should().Be(1);
            onPropertyChangedCallCount.Should().Be(1);
            passthrough.PropertyChanged -= OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, KoKoPropertyChangedEventArgs<int> e) {
            onPropertyChangedCallCount++;
        }

        [Fact]
        public void UsesSynchronizationContextIfConfigured() {
            var stored = new StoredProperty<int>(1);
            var passthrough = new PassthroughProperty<int>(stored);

            int eventsTriggeredCount = 0;
            var synchronizationContext = new MySyncContext();
            passthrough.EventSynchronizationContext = synchronizationContext;
            passthrough.PropertyChanged += (sender, args) => { eventsTriggeredCount++; };

            synchronizationContext.PostCount.Should().Be(0);
            synchronizationContext.SendCount.Should().Be(0);

            stored.Value = 2;

            eventsTriggeredCount.Should().Be(1);
            synchronizationContext.SendCount.Should().Be(1);
            synchronizationContext.PostCount.Should().Be(0);
            passthrough.EventSynchronizationContext.Should().BeSameAs(synchronizationContext);
        }

    }

    internal class MySyncContext: SynchronizationContext {

        public int PostCount;
        public int SendCount;

        public override void Post(SendOrPostCallback d, object state) {
            PostCount++;
            base.Post(d, state);
        }

        public override void Send(SendOrPostCallback d, object state) {
            SendCount++;
            base.Send(d, state);
        }

    }

}
using CoCo.Property;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class TestPassthroughProperty
    {
        [Fact]
        public void InitialValue()
        {
            var stored = new StoredProperty<int>(1);
            var passthrough = new PassthroughProperty<int>(stored);

            passthrough.Value.Should().Be(1);
        }

        [Fact]
        public void ValueIsUpdated()
        {
            var stored = new StoredProperty<int>(1);
            var passthrough = new PassthroughProperty<int>(stored);
            stored.Value.Should().Be(1);

            stored.Value = 2;
            passthrough.Value.Should().Be(2);
        }

        [Fact]
        public void EventsAreTriggered()
        {
            var stored = new StoredProperty<int>(1);
            var passthrough = new PassthroughProperty<int>(stored);
            stored.Value.Should().Be(1);

            int eventsTriggeredCount = 0;
            passthrough.PropertyChanged += (sender, args) =>
            {
                eventsTriggeredCount++;
                passthrough.Value.Should().Be(2);
            };

            stored.Value = 2;

            eventsTriggeredCount.Should().Be(1);
        }
    }
}
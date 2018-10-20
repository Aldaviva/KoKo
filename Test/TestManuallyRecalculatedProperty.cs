using System.Threading;
using FluentAssertions;
using KoKo.Property;
using Xunit;

namespace Test {

    public class TestManuallyRecalculatedProperty {

        private int calculations = 0;

        private int Calculator() {
            return Interlocked.Increment(ref calculations);
        }

        [Fact]
        public void InitialValue() {
            var property = new ManuallyRecalculatedProperty<int>(Calculator);
            calculations.Should().Be(1, "only calculated once, when constructing property");
            property.Value.Should().Be(1);
        }

        [Fact]
        public void Recalculation() {
            var property = new ManuallyRecalculatedProperty<int>(Calculator);
            property.Value.Should().Be(1);
            property.Recalculate();
            property.Value.Should().Be(2);
        }

        [Fact]
        public void EventFiredWhenRecalculating() {
            var property = new ManuallyRecalculatedProperty<int>(Calculator);
            int eventsFired = 0;
            property.PropertyChanged += (sender, args) => eventsFired++;
            property.Recalculate();
            eventsFired.Should().Be(1);
        }

        [Fact]
        public void EventNotFiredWhenRecalculatingSameValue() {
            var property = new ManuallyRecalculatedProperty<int>(() => 8);
            int eventsFired = 0;
            property.PropertyChanged += (sender, args) => eventsFired++;
            property.Recalculate();
            eventsFired.Should().Be(0, "value did not change, so no events should be fired");
        }

    }

}
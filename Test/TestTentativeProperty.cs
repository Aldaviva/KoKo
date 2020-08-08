using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using KoKo.Property;
using Xunit;

namespace Test {

    public class TestTentativeProperty {

        private readonly StoredProperty<int> parent = new StoredProperty<int>(1);
        private readonly TimeSpan shortDuration = TimeSpan.FromMilliseconds(75);
        private readonly TimeSpan longDuration = TimeSpan.FromMilliseconds(150);

        [Fact]
        public void InitialValueFromParent() {
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.Value.Should().Be(1);
        }

        [Fact]
        public void ParentPropertyUpdatesTriggerValueChangesInTentativeProperty() {
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);

            parent.Value = 2;
            tentativeProperty.Value.Should().Be(2);
        }

        [Fact]
        public void ParentPropertyUpdatesTriggerChangeEventsInTentativeProperty() {
            bool eventTriggered = false;
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.PropertyChanged += delegate { eventTriggered = true; };

            parent.Value = 2;
            eventTriggered.Should().BeTrue();
        }

        [Fact]
        public void SettingValueChangesPropertyValue() {
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.Value.Should().Be(1, "before setting");

            tentativeProperty.Value = 2;
            tentativeProperty.Value.Should().Be(2, "after setting");
        }

        [Fact]
        public void SettingValueFiresChangeEvents() {
            bool eventFired = false;
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.PropertyChanged += delegate { eventFired = true; };

            tentativeProperty.Value = 2;
            eventFired.Should().BeTrue();
        }

        [Fact]
        public void SettingValueToSameValueDoesNotFireChangeEvents() {
            bool eventFired = false;
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.PropertyChanged += delegate { eventFired = true; };

            tentativeProperty.Value = 1;
            eventFired.Should().BeFalse("value set to existing value");
        }

        [Fact]
        public Task SettingValueRevertsToParentValueAfterDuration() {
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.Value.Should().Be(1, "before setting temporary value");

            tentativeProperty.Value = 2;
            tentativeProperty.Value.Should().Be(2, "directly after setting temporary value");

            return Task.Delay(longDuration).ContinueWith(task => tentativeProperty.Value.Should().Be(1, "after waiting temporary value to revert to parent value"));
        }

        [Fact]
        public void SettingValueFiresEventsWhenReverting() {
            int eventsFired = 0;
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.Value.Should().Be(1, "before setting temporary value");
            tentativeProperty.PropertyChanged += delegate { eventsFired++; };

            tentativeProperty.Value = 2;
            tentativeProperty.Value.Should().Be(2, "directly after setting temporary value");
            eventsFired.Should().Be(1, "changed once, to temporary value");

            Thread.Sleep(longDuration);
            eventsFired.Should().Be(2, "changed once, to and from temporary value");
            tentativeProperty.Value.Should().Be(1, "after waiting temporary value to revert to parent value");
        }

        [Fact]
        public void SettingSecondTemporaryValueExtendsDurationUntilValueReverts() {
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.Value.Should().Be(1, "before setting temporary value");

            tentativeProperty.Value = 2;
            Thread.Sleep(shortDuration * 0.8);
            tentativeProperty.Value = 3;
            Thread.Sleep(shortDuration * 0.8);
            tentativeProperty.Value.Should().Be(3, "should not have reverted because we set a tentative value recently");
            Thread.Sleep(shortDuration * 0.4);
            tentativeProperty.Value.Should().Be(1, "enough time has passed that the property should have reverted to its parent value");
        }

        [Fact]
        public void SettingTemporaryValuesFiresEventsWhenReverting() {
            int eventsFired = 0;
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.PropertyChanged += delegate { eventsFired++; };

            tentativeProperty.Value = 2;
            eventsFired.Should().Be(1, "after setting once");

            Thread.Sleep(shortDuration * 0.8);
            eventsFired.Should().Be(1, "has not yet reverted yet");

            tentativeProperty.Value = 3;
            Thread.Sleep(shortDuration * 0.8);
            eventsFired.Should().Be(2, "has been set a second time, but didn't revert yet");

            Thread.Sleep(shortDuration * 0.4);
            eventsFired.Should().Be(3, "should have reverted from second temporary value to parent value");
            tentativeProperty.Value.Should().Be(1, "value should have reverted");
        }

        [Fact]
        public void DisposingPropertyCancelsReverts() {
            int eventsFired = 0;
            var tentativeProperty = new TentativeProperty<int>(parent, shortDuration);
            tentativeProperty.PropertyChanged += delegate { eventsFired++; };

            tentativeProperty.Value = 2;
            eventsFired.Should().Be(1, "one change event from explicitly setting value");
            // Thread.Sleep(shortDuration * 0.8);
            tentativeProperty.Dispose();

            Thread.Sleep(longDuration);
            tentativeProperty.Value.Should().Be(2, "value should not have reverted because we disposed the property because it could happen");
            eventsFired.Should().Be(1, "value should not have reverted because we disposed the property because it could happen");
        }

    }

}
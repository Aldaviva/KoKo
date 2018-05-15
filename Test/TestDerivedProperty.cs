using System;
using KoKo.Property;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class TestDerivedProperty
    {
        [Fact]
        public void InitialValueCalculated()
        {
            var stored = new StoredProperty<bool>(false);
            var negated = new DerivedProperty<bool>(new[] { stored }, () => !stored.Value);

            negated.Value.Should().Be(true);
        }

        [Fact]
        public void ValueRecalculatedWhenDependenciesUpdated()
        {
            var stored = new StoredProperty<string>("hello");
            var uppercased = new DerivedProperty<string>(new[] { stored }, () => stored.Value.ToUpper());
            uppercased.Value.Should().Be("HELLO");

            stored.Value = "world";
            uppercased.Value.Should().Be("WORLD");
        }

        [Fact]
        public void MultipleDependencies()
        {
            var a = new StoredProperty<bool>(false);
            var b = new StoredProperty<bool>(false);

            var deps = new Property[] { a, b };
            var and = new DerivedProperty<bool>(deps, () => a.Value && b.Value);
            var or = new DerivedProperty<bool>(deps, () => a.Value || b.Value);

            and.Value.Should().BeFalse();
            or.Value.Should().BeFalse();

            a.Value = true;
            and.Value.Should().BeFalse();
            or.Value.Should().BeTrue();

            b.Value = true;
            and.Value.Should().BeTrue();
            or.Value.Should().BeTrue();
        }

        [Fact]
        public void TransitiveDerivedDependencies()
        {
            var name = new StoredProperty<string>("world");
            var uppercased = new DerivedProperty<string>(new[] { name }, () => name.Value.ToUpper());
            var greeting = new DerivedProperty<string>(new[] { uppercased }, () => $"HELLO {uppercased.Value}!");
            greeting.Value.Should().Be("HELLO WORLD!");

            name.Value = "Ben";
            greeting.Value.Should().Be("HELLO BEN!");
        }


        [Fact]
        public void EventsTriggered()
        {
            var stored = new StoredProperty<double>(8.0);
            var abs = new DerivedProperty<double>(new[] { stored }, () => Math.Abs(stored.Value));
            abs.Value.Should().Be(8.0);

            int eventsTriggeredCount = 0;
            abs.PropertyChanged += (sender, args) =>
            {
                eventsTriggeredCount++;
                abs.Value.Should().Be(9.0);
            };

            stored.Value = 9.0;

            eventsTriggeredCount.Should().Be(1);
        }

        [Fact]
        public void SetUnchangedValueDoesNotTriggerEvents()
        {
            var stored = new StoredProperty<double>(8.0);
            var abs = new DerivedProperty<double>(new[] { stored }, () => Math.Abs(stored.Value));
            abs.Value.Should().Be(8.0);

            int eventsTriggeredCount = 0;
            abs.PropertyChanged += (sender, args) =>
            {
                eventsTriggeredCount++;
                abs.Value.Should().Be(8.0);
            };

            stored.Value = -8.0;

            eventsTriggeredCount.Should().Be(0);
        }
    }
}
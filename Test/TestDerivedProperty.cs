using System;
using System.ComponentModel;
using FluentAssertions;
using KoKo.Property;
using Xunit;

namespace Test {

    public class TestDerivedProperty {

        [Fact]
        public void InitialValueCalculated() {
            var stored = new StoredProperty<bool>();
            DerivedProperty<bool> negated = DerivedProperty<bool>.Create(stored, storedValue => !storedValue);

            negated.Value.Should().Be(true);
        }

        [Fact]
        public void ValueRecalculatedWhenDependenciesUpdated() {
            var stored = new StoredProperty<string>("hello");
            DerivedProperty<string> uppercased = DerivedProperty<string>.Create(stored, storedValue => storedValue.ToUpper());
            uppercased.Value.Should().Be("HELLO");

            stored.Value = "world";
            uppercased.Value.Should().Be("WORLD");
        }

        [Fact]
        public void MultipleDependencies() {
            var a = new StoredProperty<bool>();
            var b = new StoredProperty<bool>();

            DerivedProperty<bool> and = DerivedProperty<bool>.Create(a, b, (aValue, bValue) => aValue && bValue);
            DerivedProperty<bool> or = DerivedProperty<bool>.Create(a, b, (aValue, bValue) => aValue || bValue);

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
        public void TransitiveDerivedDependencies() {
            var name = new StoredProperty<string>("world");
            DerivedProperty<string> uppercased = DerivedProperty<string>.Create(name, nameValue => nameValue.ToUpper());
            DerivedProperty<string> greeting = DerivedProperty<string>.Create(uppercased, uppercasedValue => $"HELLO {uppercasedValue}!");
            greeting.Value.Should().Be("HELLO WORLD!");

            name.Value = "Ben";
            greeting.Value.Should().Be("HELLO BEN!");
        }

        [Fact]
        public void EventsTriggered() {
            var stored = new StoredProperty<double>(8.0);
            DerivedProperty<double> abs = DerivedProperty<double>.Create(stored, Math.Abs);
            abs.Value.Should().Be(8.0);

            int eventsTriggeredCount = 0;
            abs.PropertyChanged += (sender, args) => {
                eventsTriggeredCount++;
                abs.Value.Should().Be(9.0);
            };

            stored.Value = 9.0;

            eventsTriggeredCount.Should().Be(1);
        }

        [Fact]
        public void SetUnchangedValueDoesNotTriggerEvents() {
            var stored = new StoredProperty<double>(8.0);
            DerivedProperty<double> abs = DerivedProperty<double>.Create(stored, Math.Abs);
            abs.Value.Should().Be(8.0);

            int eventsTriggeredCount = 0;
            abs.PropertyChanged += (sender, args) => {
                eventsTriggeredCount++;
                abs.Value.Should().Be(8.0);
            };

            stored.Value = -8.0;

            eventsTriggeredCount.Should().Be(0);
        }

        [Fact]
        public void Fibonacci() {
            var zero = new StoredProperty<int>(0);
            var one = new StoredProperty<int>(1);

            var f = new Property<int>[17];
            f[0] = zero;
            f[1] = one;
            f[2] = DerivedProperty<int>.Create(f[0], f[1], (i0, i1) => i0 + i1);
            f[3] = DerivedProperty<int>.Create(f[0], f[1], f[2], (i0, i1, i2) => i1 + i2);
            f[4] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], (i0, i1, i2, i3) => i2 + i3);
            f[5] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], (i0, i1, i2, i3, i4) => i3 + i4);
            f[6] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], (i0, i1, i2, i3, i4, i5) => i4 + i5);
            f[7] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], (i0, i1, i2, i3, i4, i5, i6) => i5 + i6);
            f[8] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], f[7], (i0, i1, i2, i3, i4, i5, i6, i7) => i6 + i7);
            f[9] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], f[7], f[8], (i0, i1, i2, i3, i4, i5, i6, i7, i8) => i7 + i8);
            f[10] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], f[7], f[8], f[9], (i0, i1, i2, i3, i4, i5, i6, i7, i8, i9) => i8 + i9);
            f[11] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], f[7], f[8], f[9], f[10], (i0, i1, i2, i3, i4, i5, i6, i7, i8, i9, i10) => i9 + i10);
            f[12] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], f[7], f[8], f[9], f[10], f[11], (i0, i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11) => i10 + i11);
            f[13] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], f[7], f[8], f[9], f[10], f[11], f[12],
                (i0, i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12) => i11 + i12);
            f[14] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], f[7], f[8], f[9], f[10], f[11], f[12], f[13],
                (i0, i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, i13) => i12 + i13);
            f[15] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], f[7], f[8], f[9], f[10], f[11], f[12], f[13], f[14],
                (i0, i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, i13, i14) => i13 + i14);
            f[16] = DerivedProperty<int>.Create(f[0], f[1], f[2], f[3], f[4], f[5], f[6], f[7], f[8], f[9], f[10], f[11], f[12], f[13], f[14], f[15],
                (i0, i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, i13, i14, i15) => i14 + i15);

            f[2].Value.Should().Be(1);
            f[3].Value.Should().Be(2);
            f[4].Value.Should().Be(3);
            f[5].Value.Should().Be(5);
            f[6].Value.Should().Be(8);
            f[7].Value.Should().Be(13);
            f[8].Value.Should().Be(21);
            f[9].Value.Should().Be(34);
            f[10].Value.Should().Be(55);
            f[11].Value.Should().Be(89);
            f[12].Value.Should().Be(144);
            f[13].Value.Should().Be(233);
            f[14].Value.Should().Be(377);
            f[15].Value.Should().Be(610);
            f[16].Value.Should().Be(987);
        }

        [Fact]
        public void UnsubscribeFromBuiltInInterfaceEvent() {
            int eventsFired = 0;
            var stored = new StoredProperty<int>(1);
            DerivedProperty<int> property = DerivedProperty<int>.Create(stored, i => i + 100);
            var eventSource = (INotifyPropertyChanged) property;

            void OnValueChanged(object sender, PropertyChangedEventArgs e) {
                eventsFired++;
            }

            eventSource.PropertyChanged += OnValueChanged;

            stored.Value = 2;
            eventsFired.Should().Be(1);

            eventSource.PropertyChanged -= OnValueChanged;

            stored.Value = 3;
            eventsFired.Should().Be(1, "no additional events should have been fired after we unsubscribed");
        }

        [Fact]
        public void GetUntypedValue() {
            var stored = new StoredProperty<int>(1);
            DerivedProperty<int> property = DerivedProperty<int>.Create(stored, i => i + 100);

            var rawProperty = (Property) property;
            rawProperty.Value.Should().Be(101);
        }

    }

}
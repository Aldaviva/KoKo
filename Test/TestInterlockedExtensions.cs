using System.Collections.Generic;
using FluentAssertions;
using KoKo.Property;
using Xunit;

namespace Test {

    public class TestInterlockedExtensions {

        private int changeEventsFired = 0;

        [Fact]
        public void IncrementInt() {
            var property = new StoredProperty<int>(7);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Increment().Should().Be(8);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void IncrementLong() {
            var property = new StoredProperty<long>(7);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Increment().Should().Be(8);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void DecrementInt() {
            var property = new StoredProperty<int>(9);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Decrement().Should().Be(8);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void DecrementLong() {
            var property = new StoredProperty<long>(9);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Decrement().Should().Be(8);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void AddInt() {
            var property = new StoredProperty<int>(5);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Add(3).Should().Be(8);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void AddLong() {
            var property = new StoredProperty<long>(5);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Add(3).Should().Be(8);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void AddIntWithNoChangeDoesNotFireEvents() {
            var property = new StoredProperty<int>(8);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Add(0).Should().Be(8);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(0);
        }

        [Fact]
        public void AddLongWithNoChangeDoesNotFireEvents() {
            var property = new StoredProperty<int>(8);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Add(0).Should().Be(8);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(0);
        }

        [Fact]
        public void ExchangeInt() {
            var property = new StoredProperty<int>(7);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Exchange(8).Should().Be(7);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void ExchangeLong() {
            var property = new StoredProperty<long>(7);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Exchange(8).Should().Be(7);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void ExchangeDouble() {
            var property = new StoredProperty<double>(7);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Exchange(8, 0.0001).Should().Be(7);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void ExchangeObject() {
            var initialValue = new List<int>();
            var newValue = new List<double>();

            var property = new StoredProperty<object>(initialValue);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Exchange(newValue).Should().BeSameAs(initialValue);
            property.Value.Should().BeSameAs(newValue);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void ExchangeGeneric() {
            var initialValue = new List<int>();
            var newValue = new List<int>();

            var property = new StoredProperty<List<int>>(initialValue);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.Exchange(newValue).Should().BeSameAs(initialValue);
            property.Value.Should().BeSameAs(newValue);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void CompareExchangeInt() {
            var property = new StoredProperty<int>(7);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.CompareExchange(8, 7).Should().Be(7);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void CompareExchangeLong() {
            var property = new StoredProperty<long>(7);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.CompareExchange(8, 7).Should().Be(7);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void CompareExchangeDouble() {
            var property = new StoredProperty<double>(7);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.CompareExchange(8, 7, 0.0001).Should().Be(7);
            property.Value.Should().Be(8);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void CompareExchangeObject() {
            var initialValue = new List<int>();
            var newValue = new List<double>();
            var property = new StoredProperty<object>(initialValue);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.CompareExchange(newValue, initialValue).Should().BeSameAs(initialValue);
            property.Value.Should().BeSameAs(newValue);
            changeEventsFired.Should().Be(1);
        }

        [Fact]
        public void CompareExchangeGeneric() {
            var initialValue = new List<int>();
            var newValue = new List<int>();
            var property = new StoredProperty<List<int>>(initialValue);
            property.PropertyChanged += delegate { changeEventsFired++; };
            property.CompareExchange(newValue, initialValue).Should().BeSameAs(initialValue);
            property.Value.Should().BeSameAs(newValue);
            changeEventsFired.Should().Be(1);
        }

    }

}
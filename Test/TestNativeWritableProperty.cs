using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FluentAssertions;
using KoKo.Property;
using Xunit;

namespace Test {

    public class TestNativeWritableProperty {

        [Fact]
        public void InitialValue() {
            int nativeEvents = 0, kokoEvents = 0;
            var myNativeProperty = new MyWritableNativePropertyClass();
            myNativeProperty.PropertyChanged += delegate { nativeEvents++; };

            myNativeProperty.Greeting = "hello";
            nativeEvents.Should().Be(1);

            var kokoProperty = new NativeReadableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };
            kokoProperty.Value.Should().Be("hello");
            kokoEvents.Should().Be(0, "not changed while koko property existed yet");
        }

        [Fact]
        public void NativeChange() {
            int nativeEvents = 0, kokoEvents = 0;
            var myNativeProperty = new MyWritableNativePropertyClass();
            myNativeProperty.PropertyChanged += delegate { nativeEvents++; };
            myNativeProperty.Greeting = "hello";

            var kokoProperty = new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };
            kokoProperty.Value.Should().Be("hello");
            kokoEvents.Should().Be(0);

            myNativeProperty.Greeting = "howdy";
            kokoProperty.Value.Should().Be("howdy");
            kokoEvents.Should().Be(1);
            nativeEvents.Should().Be(2);
        }

        [Fact]
        public void KokoChange() {
            int nativeEvents = 0, kokoEvents = 0;
            var myNativeProperty = new MyWritableNativePropertyClass();
            myNativeProperty.PropertyChanged += delegate { nativeEvents++; };
            myNativeProperty.Greeting = "hello";

            var kokoProperty = new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };
            kokoEvents.Should().Be(0);
            kokoProperty.Value.Should().Be("hello");

            kokoProperty.Value = "howdy";
            kokoProperty.Value.Should().Be("howdy");
            myNativeProperty.Greeting.Should().Be("howdy");
            kokoEvents.Should().Be(1);
            nativeEvents.Should().Be(2);
        }

        [Fact]
        public void GetNotVisible() {
            var myNativeProperty = new MyWritableNativePropertyClass();

            Action thrower = () => new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.SetOnly));
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void SetNotVisible() {
            var myNativeProperty = new MyWritableNativePropertyClass();

            Action thrower = () => new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.GetOnly));
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void WrongType() {
            var myNativeProperty = new MyWritableNativePropertyClass();

            Action thrower = () => new NativeWritableProperty<int>(myNativeProperty, nameof(myNativeProperty.Greeting));
            thrower.Should().Throw<ArgumentException>();
        }

    }

    internal class MyWritableNativePropertyClass: INotifyPropertyChanged {

        private string greeting;

        public string Greeting {
            get => greeting;
            set {
                if (greeting != value) {
                    greeting = value;
                    OnPropertyChanged();
                }
            }
        }

        public string GetOnly => "getOnly";
        private string setOnly;
        public string SetOnly {
            set => setOnly = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
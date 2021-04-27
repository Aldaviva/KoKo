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
            int nativeEvents     = 0, kokoEvents = 0;
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
            int nativeEvents     = 0, kokoEvents = 0;
            var myNativeProperty = new MyWritableNativePropertyClass();
            myNativeProperty.PropertyChanged += delegate { nativeEvents++; };
            myNativeProperty.Greeting        =  "hello";

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
        public void WrongPropertyNativeChange() {
            int nativeEvents     = 0, kokoEvents = 0;
            var myNativeProperty = new MyWritableNativePropertyClass();
            myNativeProperty.PropertyChanged += delegate { nativeEvents++; };
            myNativeProperty.PartingPhrase   =  "goodbye";

            var kokoProperty = new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };
            kokoProperty.Value.Should().BeNull();
            kokoEvents.Should().Be(0);

            myNativeProperty.PartingPhrase = "arrivederci";
            kokoProperty.Value.Should().BeNull();
            kokoEvents.Should().Be(0);
            nativeEvents.Should().Be(2);
        }

        [Fact]
        public void KokoChange() {
            int nativeEvents     = 0, kokoEvents = 0;
            var myNativeProperty = new MyWritableNativePropertyClass();
            myNativeProperty.PropertyChanged += delegate { nativeEvents++; };
            myNativeProperty.Greeting        =  "hello";

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

            // ReSharper disable once ObjectCreationAsStatement we want to see the constructor throw an exception
            Action thrower = () => new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.SetOnly));
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void SetNotVisible() {
            var myNativeProperty = new MyWritableNativePropertyClass();

            // ReSharper disable once ObjectCreationAsStatement we want to see the constructor throw an exception
            Action thrower = () => new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.GetOnly));
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void WrongType() {
            var myNativeProperty = new MyWritableNativePropertyClass { Greeting = "hi" };

            // ReSharper disable once ObjectCreationAsStatement we want to see the constructor throw an exception
            Action thrower = () => new NativeWritableProperty<int>(myNativeProperty, nameof(myNativeProperty.Greeting));
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void NonNotifyingPropertyInitialValue() {
            int nativeEvents     = 0, kokoEvents = 0;
            var myNativeProperty = new MyWritableNonNotifyingNativePropertyClass();
            myNativeProperty.GreetingChanged += delegate { nativeEvents++; };

            myNativeProperty.Greeting = "hello";
            nativeEvents.Should().Be(1);

            var kokoProperty = new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting), nameof(myNativeProperty.GreetingChanged));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };
            kokoProperty.Value.Should().Be("hello");
            kokoEvents.Should().Be(0, "not changed while koko property existed yet");
        }

        [Fact]
        public void NonNotifyingPropertyValueChanged() {
            int nativeEvents = 0, kokoEvents = 0;

            var myNativeProperty = new MyWritableNonNotifyingNativePropertyClass();
            myNativeProperty.Greeting        =  "hello";
            myNativeProperty.GreetingChanged += delegate { nativeEvents++; };

            var kokoProperty = new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting), nameof(myNativeProperty.GreetingChanged));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };

            myNativeProperty.Greeting = "howdy";
            kokoProperty.Value.Should().Be("howdy");
            kokoEvents.Should().Be(1);
            nativeEvents.Should().Be(1);
        }

        [Fact]
        public void NonNotifyingPropertyValueSettable() {
            int nativeEvents = 0, kokoEvents = 0;

            var myNativeProperty = new MyWritableNonNotifyingNativePropertyClass();
            myNativeProperty.Greeting        =  "hello";
            myNativeProperty.GreetingChanged += delegate { nativeEvents++; };

            var kokoProperty = new NativeWritableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting), nameof(myNativeProperty.GreetingChanged));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };

            kokoProperty.Value = "howdy";
            kokoProperty.Value.Should().Be("howdy");
            kokoEvents.Should().Be(1);
            nativeEvents.Should().Be(1);
        }

    }

    internal class MyWritableNativePropertyClass: INotifyPropertyChanged {

        private string greeting;

        public string Greeting {
            get => greeting;
            set {
                if (greeting == value) return;
                greeting = value;
                OnPropertyChanged();
            }
        }

        public string GetOnly => "getOnly";

        // ReSharper disable once NotAccessedField.Local it's for a test
        private string setOnly;

        public string SetOnly {
            // ReSharper disable once MemberCanBePrivate.Global it actually can't, the lack of a public getter causes a compilation error with a private setter
            set => setOnly = value;
        }

        private string partingPhrase;

        public string PartingPhrase {
            get => partingPhrase;
            set {
                if (partingPhrase == value) return;
                partingPhrase = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    internal class MyWritableNonNotifyingNativePropertyClass {

        private string greeting;

        public string Greeting {
            get => greeting;
            set {
                if (greeting == value) return;
                greeting = value;
                GreetingChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler GreetingChanged;

    }

}
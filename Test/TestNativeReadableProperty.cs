﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using FluentAssertions;
using KoKo.Property;
using Xunit;

namespace Test {

    public class TestNativeReadableProperty {

        [Fact]
        public void InitialValue() {
            int nativeEvents     = 0, kokoEvents = 0;
            var myNativeProperty = new MyReadableNativePropertyClass();
            myNativeProperty.PropertyChanged += delegate { nativeEvents++; };

            myNativeProperty.ChangeGreeting("hello");
            nativeEvents.Should().Be(1);

            var kokoProperty = new NativeReadableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };
            kokoProperty.Value.Should().Be("hello");
            kokoEvents.Should().Be(0, "not changed while koko property existed yet");
        }

        [Fact]
        public void NativeChange() {
            int nativeEvents     = 0, kokoEvents = 0;
            var myNativeProperty = new MyReadableNativePropertyClass();
            myNativeProperty.PropertyChanged += delegate { nativeEvents++; };
            myNativeProperty.ChangeGreeting("hello");

            var kokoProperty = new NativeReadableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };

            myNativeProperty.ChangeGreeting("howdy");
            kokoProperty.Value.Should().Be("howdy");
            kokoEvents.Should().Be(1);
            nativeEvents.Should().Be(2);
        }

        [Fact]
        public void GetNotVisible() {
            var myNativeProperty = new MyReadableNativePropertyClass();

            // ReSharper disable once ObjectCreationAsStatement we want to see the constructor throw an exception
            Action thrower = () => new NativeReadableProperty<string>(myNativeProperty, nameof(myNativeProperty.SetOnly));
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void WrongType() {
            var myNativeProperty = new MyReadableNativePropertyClass();
            myNativeProperty.ChangeGreeting("hi");

            // ReSharper disable once ObjectCreationAsStatement we want to see the constructor throw an exception
            Action thrower = () => new NativeReadableProperty<int>(myNativeProperty, nameof(myNativeProperty.Greeting));
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void WrongProperty() {
            var myNativeProperty = new MyReadableNativePropertyClass();
            myNativeProperty.ChangeGreeting("hi");

            // ReSharper disable once ObjectCreationAsStatement we want to see the constructor throw an exception
            Action thrower = () => new NativeReadableProperty<int>(myNativeProperty, nameof(myNativeProperty.Greeting) + "_wrong");
            thrower.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void NonNotifyingPropertyInitialValue() {
            int nativeEvents     = 0, kokoEvents = 0;
            var myNativeProperty = new MyNonNotifyingNativePropertyClass();
            myNativeProperty.GreetingChanged += delegate { nativeEvents++; };

            myNativeProperty.ChangeGreeting("hello");
            nativeEvents.Should().Be(1);

            var kokoProperty = new NativeReadableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting), nameof(myNativeProperty.GreetingChanged));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };
            kokoProperty.Value.Should().Be("hello");
            kokoEvents.Should().Be(0, "not changed while koko property existed yet");
        }

        [Fact]
        public void NonNotifyingPropertyValueChanged() {
            int nativeEvents = 0, kokoEvents = 0;

            var myNativeProperty = new MyNonNotifyingNativePropertyClass();
            myNativeProperty.ChangeGreeting("hello");
            myNativeProperty.GreetingChanged += delegate { nativeEvents++; };

            var kokoProperty = new NativeReadableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting), nameof(myNativeProperty.GreetingChanged));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };

            myNativeProperty.ChangeGreeting("howdy");
            kokoProperty.Value.Should().Be("howdy");
            kokoEvents.Should().Be(1);
            nativeEvents.Should().Be(1);
        }

        [Fact]
        public void NonNotifyingPropertyImplicitEventName() {
            int nativeEvents = 0, kokoEvents = 0;

            var myNativeProperty = new MyNonNotifyingNativePropertyClass();
            myNativeProperty.ChangeGreeting("hello");
            myNativeProperty.GreetingChanged += delegate { nativeEvents++; };

            var kokoProperty = new NativeReadableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };

            myNativeProperty.ChangeGreeting("howdy");
            kokoProperty.Value.Should().Be("howdy");
            kokoEvents.Should().Be(1);
            nativeEvents.Should().Be(1);
        }

        [Fact]
        public void WindowsFormsPropertyValueChanged() {
            int nativeEvents = 0, kokoEvents = 0;

            var toolStripStatusLabel = new ToolStripStatusLabel { Text = "ready" };
            toolStripStatusLabel.TextChanged += delegate { nativeEvents++; };

            var kokoProperty = new NativeReadableProperty<string>(toolStripStatusLabel, nameof(toolStripStatusLabel.Text), nameof(toolStripStatusLabel.TextChanged));
            kokoProperty.PropertyChanged += delegate { kokoEvents++; };

            toolStripStatusLabel.Text = "howdy";
            kokoProperty.Value.Should().Be("howdy");
            kokoEvents.Should().Be(1);
            nativeEvents.Should().Be(1);
        }

        [Fact]
        public void WrongEventName() {
            var myNativeProperty = new MyNonNotifyingNativePropertyClass();
            myNativeProperty.ChangeGreeting("hello");
            // ReSharper disable once ObjectCreationAsStatement constructor throws
            Action thrower = () => new NativeReadableProperty<string>(myNativeProperty, nameof(myNativeProperty.Greeting), nameof(myNativeProperty.GreetingChanged) + "_wrong");
            thrower.Should().Throw<ArgumentException>();
        }

    }

    internal class MyReadableNativePropertyClass: INotifyPropertyChanged {

        public string Greeting { get; private set; }

        // ReSharper disable once NotAccessedField.Local it's set only for a test
        private string setOnly;

        public string SetOnly {
            // ReSharper disable once MemberCanBePrivate.Global it actually can't, the lack of a public getter causes a compilation error with a private setter
            set => setOnly = value;
        }

        public void ChangeGreeting(string newGreeting) {
            Greeting = newGreeting;
            OnPropertyChanged(nameof(Greeting));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    internal class MyNonNotifyingNativePropertyClass {

        public string Greeting { get; private set; }

        public event EventHandler GreetingChanged;

        public void ChangeGreeting(string newGreeting) {
            Greeting = newGreeting;
            GreetingChanged?.Invoke(this, EventArgs.Empty);
        }

    }

}
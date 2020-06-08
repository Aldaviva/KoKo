using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using KoKo.Property;

namespace Test {

    public class ReadmeSamples {

        private void storedProperty() {
            var stored = new StoredProperty<string>("world");
            Console.WriteLine($"Hello {stored.Value}"); // Hello world
            stored.Value = "world!";
            Console.WriteLine($"Hello {stored.Value}"); // Hello world!
        }

        private void derivedProperty() {
            var backing = new StoredProperty<int>(8);
            DerivedProperty<int> derived = DerivedProperty<int>.Create(backing, (bValue) => System.Math.Abs(bValue));
            Console.WriteLine($"The absolute value of {backing.Value} is {derived.Value}."); // The absolute value of 8 is 8.
            backing.Value = -9;
            Console.WriteLine($"The absolute value of {backing.Value} is {derived.Value}."); // The absolute value of -9 is 9.
        }

        private void passthroughProperty() {
            var backing = new StoredProperty<double>(3.0);
            var passthrough = new PassthroughProperty<double>(backing);
            Console.WriteLine($"{passthrough.Value} liters"); // 3 liters
            backing.Value = 5.0;
            Console.WriteLine($"{passthrough.Value} liters"); // 5 liters
        }

        private void tentativeProperty() {
            var backing = new StoredProperty<int>(8);
            var tentative = new TentativeProperty<int>(backing, TimeSpan.FromMilliseconds(500));
            Console.WriteLine(tentative.Value); // 8
            backing.Value = 9;
            Console.WriteLine(tentative.Value); // 9
            tentative.Value = 10;
            backing.Value = 11;
            Console.WriteLine(tentative.Value); // 10
            Thread.Sleep(1000);
            Console.WriteLine(tentative.Value); // 11
        }

        private void connectableProperty() {
            var connectable = new ConnectableProperty<int>(0);
            var a = new StoredProperty<int>(8);
            Console.WriteLine(connectable.Value); // 0
            connectable.Connect(a);
            Console.WriteLine(connectable.Value); // 8
        }

        private void manuallyRecalculatedProperty() {
            var manuallyRecalculated = new ManuallyRecalculatedProperty<long>(() => DateTimeOffset.Now.ToUnixTimeMilliseconds());
            Console.WriteLine(manuallyRecalculated); // 1591651725420
            Thread.Sleep(1000);
            manuallyRecalculated.Recalculate();
            Console.WriteLine(manuallyRecalculated); // 1591651726420
        }

        private void multiLevelProperty() {
            var person = new Person("FirstName", "LastName");
            var currentUser = new StoredProperty<Person>(person);
            var currentUserFullName = new MultiLevelProperty<string>(() => currentUser.Value.fullName);
            Console.WriteLine($"Welcome, {currentUserFullName.Value}"); // Welcome, FirstName LastName
        }

        private void nativeReadableProperty() {
            var nativePropertyObject = new NativePropertyClass { nativeProperty = 8 };
            var kokoProperty = new NativeReadableProperty<int>(nativePropertyObject, nameof(NativePropertyClass.nativeProperty));
            Console.WriteLine(kokoProperty.Value); // 8
        }

        private void nativeWritableProperty() {
            var nativePropertyObject = new NativePropertyClass { nativeProperty = 8 };
            var kokoProperty = new NativeWritableProperty<int>(nativePropertyObject, nameof(NativePropertyClass.nativeProperty));
            Console.WriteLine(kokoProperty.Value); // 8
            kokoProperty.Value = 9;
            Console.WriteLine(nativePropertyObject.nativeProperty); // 9
        }

    }

    internal class Person {

        private readonly StoredProperty<string> firstName;
        private readonly StoredProperty<string> lastName;
        internal readonly Property<string> fullName;

        internal Person(string firstName, string lastName) {
            this.firstName = new StoredProperty<string>(firstName);
            this.lastName = new StoredProperty<string>(lastName);

            fullName = DerivedProperty<string>.Create(this.firstName, this.lastName, (first, last) => $"{first} {last}");
        }

        private void changeFirstName(string newFirstName) {
            firstName.Value = newFirstName;
        }

    }

    internal class NativePropertyClass: INotifyPropertyChanged {

        public int nativeProperty { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
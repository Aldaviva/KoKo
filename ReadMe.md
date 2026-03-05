<img src="https://raw.githubusercontent.com/Aldaviva/KoKo/master/KoKo/icon.png" height="24" alt="KoKo logo" /> KoKo
===

[![Package Version](https://img.shields.io/nuget/v/KoKo?logo=nuget)](https://www.nuget.org/packages/KoKo/) [![NuGet Gallery Download Count](https://img.shields.io/nuget/dt/KoKo?logo=nuget&color=blue
)](https://www.nuget.org/packages/KoKo/) ![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/Aldaviva/KoKo/dotnetpackage.yml?branch=master&logo=github) [![Testspace](https://img.shields.io/testspace/tests/Aldaviva/Aldaviva:KoKo/master?passed_label=passing&failed_label=failing&logo=data%3Aimage%2Fsvg%2Bxml%3Bbase64%2CPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCA4NTkgODYxIj48cGF0aCBkPSJtNTk4IDUxMy05NCA5NCAyOCAyNyA5NC05NC0yOC0yN3pNMzA2IDIyNmwtOTQgOTQgMjggMjggOTQtOTQtMjgtMjh6bS00NiAyODctMjcgMjcgOTQgOTQgMjctMjctOTQtOTR6bTI5My0yODctMjcgMjggOTQgOTQgMjctMjgtOTQtOTR6TTQzMiA4NjFjNDEuMzMgMCA3Ni44My0xNC42NyAxMDYuNS00NFM1ODMgNzUyIDU4MyA3MTBjMC00MS4zMy0xNC44My03Ni44My00NC41LTEwNi41UzQ3My4zMyA1NTkgNDMyIDU1OWMtNDIgMC03Ny42NyAxNC44My0xMDcgNDQuNXMtNDQgNjUuMTctNDQgMTA2LjVjMCA0MiAxNC42NyA3Ny42NyA0NCAxMDdzNjUgNDQgMTA3IDQ0em0wLTU1OWM0MS4zMyAwIDc2LjgzLTE0LjgzIDEwNi41LTQ0LjVTNTgzIDE5Mi4zMyA1ODMgMTUxYzAtNDItMTQuODMtNzcuNjctNDQuNS0xMDdTNDczLjMzIDAgNDMyIDBjLTQyIDAtNzcuNjcgMTQuNjctMTA3IDQ0cy00NCA2NS00NCAxMDdjMCA0MS4zMyAxNC42NyA3Ni44MyA0NCAxMDYuNVMzOTAgMzAyIDQzMiAzMDJ6bTI3NiAyODJjNDIgMCA3Ny42Ny0xNC44MyAxMDctNDQuNXM0NC02NS4xNyA0NC0xMDYuNWMwLTQyLTE0LjY3LTc3LjY3LTQ0LTEwN3MtNjUtNDQtMTA3LTQ0Yy00MS4zMyAwLTc2LjY3IDE0LjY3LTEwNiA0NHMtNDQgNjUtNDQgMTA3YzAgNDEuMzMgMTQuNjcgNzYuODMgNDQgMTA2LjVTNjY2LjY3IDU4NCA3MDggNTg0em0tNTU3IDBjNDIgMCA3Ny42Ny0xNC44MyAxMDctNDQuNXM0NC02NS4xNyA0NC0xMDYuNWMwLTQyLTE0LjY3LTc3LjY3LTQ0LTEwN3MtNjUtNDQtMTA3LTQ0Yy00MS4zMyAwLTc2LjgzIDE0LjY3LTEwNi41IDQ0UzAgMzkxIDAgNDMzYzAgNDEuMzMgMTQuODMgNzYuODMgNDQuNSAxMDYuNVMxMDkuNjcgNTg0IDE1MSA1ODR6IiBmaWxsPSIjZmZmIi8%2BPC9zdmc%2B)](https://aldaviva.testspace.com/spaces/337005) [![Coveralls](https://img.shields.io/coveralls/github/Aldaviva/KoKo?logo=coveralls)](https://coveralls.io/github/Aldaviva/KoKo?branch=master)

*Knockout for Cocoa, for C#*

KoKo lets you create `Property` objects as members of your model classes.

Unlike [native C# properties](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties), KoKo `Property` objects automatically fire change [events](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/). They can be composed from multiple other Properties without the dependencies being aware of the dependents, and without writing any boilerplate event handling code. They are compatible with native C# properties and events, as well as with WPF and Windows Forms databinding.

These properties are very similar to what you would find in [Knockout](https://knockoutjs.com/), [MobX](https://mobx.js.org/), and WPF's [`DependencyProperty`](https://learn.microsoft.com/en-us/dotnet/api/system.windows.dependencyproperty). They do not rely on a presentation layer like WPF, and they do not require you to import and understand a large, overblown, confusing library like [.NET Reactive Extensions/Rx.NET](https://dotnetfoundation.org/projects/reactive-extensions).

This library was ported from an open-source Swift library by [@abrindam](https://github.com/abrindam) called KoKo (which means "Knockout for Cocoa"), which was later renamed to [Yoyo](https://github.com/onelittlefish/Yoyo) because "KoKo" and "Cocoa" are homophones and thus verbally indistinguishable.

<!-- MarkdownTOC autolink="true" bracket="round" autoanchor="false" levels="1,2,3" -->

- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
    - [Example](#example)
- [Types of Properties](#types-of-properties)
    - [**`StoredProperty`**](#storedproperty)
    - [**`DerivedProperty`**](#derivedproperty)
    - [`ConnectableProperty`](#connectableproperty)
    - [`ManuallyRecalculatedProperty`](#manuallyrecalculatedproperty)
    - [`MultiLevelProperty`](#multilevelproperty)
    - [`NativeReadableProperty`](#nativereadableproperty)
    - [`NativeWritableProperty`](#nativewritableproperty)
    - [`PassthroughProperty`](#passthroughproperty)
    - [`TentativeProperty`](#tentativeproperty)
- [Events on Properties](#events-on-properties)
- [Threading](#threading)

<!-- /MarkdownTOC -->

## Requirements
- Any .NET runtime that supports [.NET Standard 2.0](https://learn.microsoft.com/en-us/dotnet/standard/net-standard)
    - .NET 5 or later
    - .NET Core 2.0 or later
    - .NET Framework 4.6.2 or later

## Installation
```ps1
dotnet add package KoKo
```

## Usage
1. Create a `class` to act as your business or view model.
1. Import the `KoKo.Property` namespace.
1. Add some `Property` fields, one for each piece of data you want to represent.
1. Get and set their `Value`.

### Example
This is a just a silly, simple example. [Don't actually represent people's names this way.](https://www.w3.org/International/questions/qa-personal-names)

```cs
using KoKo.Property;

namespace MyProject;

public class Person {

    private StoredProperty<string> FirstName { get; }
    private StoredProperty<string> LastName { get; }
    public Property<string> FullName { get; }

    public Person(string firstName, string lastName) {
        FirstName = new StoredProperty<string>(firstName);
        LastName = new StoredProperty<string>(lastName);
        FullName = DerivedProperty<string>.Create(FirstName, LastName, (first, last) => $"{first} {last}");
    }

    public void SetFirstName(string firstName) {
        FirstName.Value = firstName;
    }

}
```

#### Programmatic access
Now you can get a `person` object's autogenerated full name,

```cs
var person = new Person("Alice", "Smith");
Console.WriteLine(person.FullName.Value); // Alice Smith
```

and if you change a dependency value, the dependent full name will be automatically updated.

```cs
person.SetFirstName("Bob");
Console.WriteLine(person.FullName.Value); // Bob Smith
```

#### Data-bound access

You can also use Properties with databinding:

```xaml
<!-- WPF -->
<Label Content="{Binding FullName.Value}" />
```

```cs
// Windows Forms
private void Form1_Load(object sender, System.EventArgs e) {
    personNameLabel.DataBindings.Add("Text", model.FullName, "Value");
}
```

Remember to use the `Value` property in your databinding declarations, otherwise you won't get the property's value.

## Types of Properties

### **`StoredProperty`**

- Stores a single value in memory
- Value can be get or set imperatively
- Similar to a native C# property, except you don't have to [implement `INotifyPropertyChanged` yourself](https://learn.microsoft.com/en-us/dotnet/framework/winforms/how-to-implement-the-inotifypropertychanged-interface)
- You can perform [atomic operations](https://learn.microsoft.com/en-us/dotnet/api/system.threading.interlocked#methods) on a `StoredProperty` value using `Increment()`, `Decrement()`, `Add(value)`, `Exchange(value)`, or `CompareExchange(possibleNewValue, assignIfOldValueEquals)`

```cs
var a = new StoredProperty<string>("world");
Console.WriteLine($"Hello {a.Value}"); // Hello world
a.Value = "world!";
Console.WriteLine($"Hello {a.Value}"); // Hello world!
```

### **`DerivedProperty`**

- Automatically calculates its value from one or more other properties (_dependencies_), which can be any type of KoKo property
- You don't need to manually invoke event handlers on the dependencies when their values change, this property will automatically recalculate its value instead
- You cannot set its value directly, but you can get the value just like a [`StoredProperty`](#storedProperty)
- Unlike other KoKo property classes, you construct `DerivedProperty` instances using the `DerivedProperty.Create()` factory method instead of a constructor. This allows you to pass a type-safe calculator function that doesn't capture the dependency properties.

```cs
var b = new StoredProperty<int>(8);
DerivedProperty<int> absoluteValueB = DerivedProperty<int>.Create(b, (bValue) => System.Math.Abs(bValue));
Console.WriteLine($"The absolute value of {b.Value} is {absoluteValueB.Value}."); // The absolute value of 8 is 8.
b.Value = -9;
Console.WriteLine($"The absolute value of {b.Value} is {absoluteValueB.Value}."); // The absolute value of -9 is 9.
```

### `ConnectableProperty`
- Like a [`PassthroughProperty`](#passthroughProperty), except the dependent `ConnectableProperty` has its dependency `Property` passed to it instead of depending upon it at creation time
- You can also pass a constant value instead of a `Property`, which is more convenient that constructing a whole new `StoredProperty` instance to hold that constant
- Useful for inversion of control where the dependent must not be aware of how to obtain its dependencies
- Can be reconnected and disconnected multiple times, to allow reconfiguration

```cs
var connectable = new ConnectableProperty<int>(0);
var a = new StoredProperty<int>(8);
var b = 9;
Console.WriteLine(connectable.Value); // 0
connectable.Connect(a);
Console.WriteLine(connectable.Value); // 8
connectable.Connect(b);
Console.WriteLine(connectable.Value); // 9
```

### `ManuallyRecalculatedProperty`
- Like a [`DerivedProperty`](#derivedProperty), except instead of recalculating its value based on changes to dependency properties, you must manually instruct the property to recalculate its value
- Useful when the dependencies do not have a way to expose change events
- Useful when the dependencies are constantly changing and you don't care about every change
- Useful when the recalculation is very expensive and you want to reduce how often it is run
- Alternatively, if you want to reuse the same calculator, expose a parameterless constructor, or parameterize multiple instances, you can subclass `ManuallyRecalculatedProperty<T>`, call the parameterless super-constructor, and override the `ComputeValue()` method with your calculator logic, rather than passing a calculator function to the super-constructor.

```cs
var manuallyRecalculated = new ManuallyRecalculatedProperty<long>(() => DateTimeOffset.Now.ToUnixTimeMilliseconds());
Console.WriteLine(manuallyRecalculated); // 1591651725420
Thread.Sleep(1000);
manuallyRecalculated.Recalculate();
Console.WriteLine(manuallyRecalculated); // 1591651726420
```

### `MultiLevelProperty`
- Like a [`PassthroughProperty`](#passthroughProperty), except it gets a property value nested at an arbitrary depth
- Useful if you have an object graph with properties whose values are classes with other properties
- Useful if the top-level reference may change to a different instance (which person is currently logged in), but the property chain you want to refer to is always the same (the full name of the currently logged-in person)

```cs
var person = new Person("FirstName", "LastName");
var currentUser = new StoredProperty<Person>(person);
var currentUserFullName = new MultiLevelProperty<string>(() => currentUser.Value.fullName);
Console.WriteLine($"Welcome, {currentUserFullName.Value}"); // Welcome, FirstName LastName
```

### `NativeReadableProperty`
- Useful for interoperation with C# classes, whether they expose property value changes using [`INotifyPropertyChanged`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) or other events

```cs
// MyNativePropertyClass implements INotifyPropertyChanged and fires PropertyChanged events
var nativePropertyObject = new MyNativePropertyClass { NativeProperty = 8 };
var kokoProperty = new NativeReadableProperty<int>(nativePropertyObject, nameof(NativePropertyClass.NativeProperty));
Console.WriteLine(kokoProperty.Value); // 8
```

- If the class does not implement `INotifyPropertyChanged`, this class assumes that it should listen for `*Changed` events that are named after the native property, so that it can tell when the native property value changes. For example, given the `Text` property on the `ToolStripStatusLabel` class, `NativeReadableProperty` will listen for `TextChanged` events.

```cs
// ToolStripStatusLabel does not implement INotifyPropertyChanged, and instead fires TextChanged events
var toolStripStatusLabel = new ToolStripStatusLabel { Text = "ready" };
// The event name TextChanged is inferred from the Text property name
var kokoProperty = new NativeReadableProperty<string>(toolStripStatusLabel, nameof(toolStripStatusLabel.Text));
```

- If the event name does not follow the `propertyName + "Changed"` naming convention, you may override it with the optional third `nativeEventName` parameter.

```cs
// MyNativePropertyClass2 does not implement INotifyPropertyChanged, and instead fires custom NativePropertyChanged events
var nativePropertyObject = new MyNativePropertyClass2 { NativeProperty = 8 };
var kokoProperty = new NativeReadableProperty<int>(nativePropertyObject, nameof(NativePropertyClass2.NativeProperty),
    nameof(NativePropertyClass2.NativePropertyChanged));
Console.WriteLine(kokoProperty.Value); // 8
```

### `NativeWritableProperty`
- Like a [`NativeReadableProperty`](#nativeReadableProperty), except you can also change the value
- Useful when the native C# property has an accessible setter

```cs
// MyNativePropertyClass implements INotifyPropertyChanged and fires PropertyChanged events
var nativePropertyObject = new MyNativePropertyClass { NativeProperty = 8 };
var kokoProperty = new NativeWritableProperty<int>(nativePropertyObject, nameof(NativePropertyClass.NativeProperty));
Console.WriteLine(kokoProperty.Value); // 8
kokoProperty.Value = 9;
Console.WriteLine(nativePropertyObject.NativeProperty); // 9
```

```cs
// MyNativePropertyClass2 does not implement INotifyPropertyChanged, and instead fires custom NativePropertyChanged events
var nativePropertyObject = new MyNativePropertyClass2 { nativeProperty = 8 };
var kokoProperty = new NativeWritableProperty<int>(nativePropertyObject, nameof(NativePropertyClass2.NativeProperty)); // Implicit event name NativePropertyChanged
Console.WriteLine(kokoProperty.Value); // 8
kokoProperty.Value = 9;
Console.WriteLine(nativePropertyObject.NativeProperty); // 9
```

```cs
// MyNativePropertyClass2 does not implement INotifyPropertyChanged, and instead fires custom NativePropertyChanged events
var nativePropertyObject = new MyNativePropertyClass2 { nativeProperty = 8 };
var kokoProperty = new NativeWritableProperty<int>(nativePropertyObject, nameof(NativePropertyClass2.NativeProperty),
    nameof(NativePropertyClass2.NativePropertyChanged)); // Explicit event name NativePropertyChanged
Console.WriteLine(kokoProperty.Value); // 8
kokoProperty.Value = 9;
Console.WriteLine(nativePropertyObject.NativeProperty); // 9
```

### `PassthroughProperty`

- Like a [`DerivedProperty`](#derivedProperty), except it depends on a single property and does not transform the value at all
- Useful for organizing your class structure and defining boundaries of knowledge or responsibility

```cs
var backing = new StoredProperty<double>(3.0);
var passthrough = new PassthroughProperty<double>(a);
Console.WriteLine($"{passthrough.Value} liters"); // 3 liters
backing.Value = 5.0;
Console.WriteLine($"{passthrough.Value} liters"); // 5 liters
```

### `TentativeProperty`
- Like a [`PassthroughProperty`](#passthroughProperty), except you can supply a temporary overriding value, which it will use for a specified duration before reverting to the passthrough value
- Useful for dealing with changes which require a long time to take effect, but you want to make it look like it took effect immediately, while still allowing it to be eventually consistent if the change fails.
- For example, clicks on a button that performs a slow remote operation which changes the button's state, but you want the button's state to update early, before the operation completes, so that the user doesn't think their click was ignored and knows what state the system is trying to reach.

```cs
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
```

## Events on Properties

You can subscribe to events that are fired when any `Property`'s value changes.

If you want to react to a property changing by changing some other data, you may want to use a `DerivedProperty` or similar, because all value change dependencies will be consistent and probably simpler to understand.

On the other hand, if you want to react to a property changing by taking some action, then you can listen for the `PropertyChanged` event:

```cs
var property = new StoredProperty<int>(1);
property.PropertyChanged += (object sender, KoKoPropertyChangedEventArgs<int> args) => {
    Console.WriteLine($"Property value changed from {args.OldValue} to {args.NewValue}.");
};
property.Value = 2; // Property value changed from 1 to 2.
```

If you need to pass a KoKo `Property` to a consumer that only accepts `INotifyPropertyChanged`, this interface is also implemented.

```cs
var property = new StoredProperty<int>(1);
((INotifyPropertyChanged property).PropertyChanged += (object sender, PropertyChangedEventArgs args) => {
    Console.WriteLine($"Property value changed to {property.Value}.");
};
property.Value = 2; // Property value changed to 2.
```

## Threading

You may want the property changed event handlers to run on a different thread than the one that caused the property value to change in the first place. This is especially important for updating UI controls, since Windows Forms and WPF only allow UI updates on the main thread, whether the update is imperative or declarative (data binding).

To accomplish this, set `EventSynchronizationContext` on your KoKo `Property` to [`SynchronizationContext.Current`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.synchronizationcontext.current).

Now, whenever the Property value changes, even if the change happened on a background thread, the event handlers will run in that [`SynchronizationContext`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.synchronizationcontext), so if you have a WPF control bound to that Property value, it will run in the correct WPF [`Dispatcher`](https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcher).

- All KoKo event handlers run synchronously, whether on the original thread or through a `SynchronizationContext`
- All KoKo Properties can have their `EventSynchronizationContext` changed, not just `PassthroughProperty`

```cs
var backing = new StoredProperty<double>(3.0);
var passthrough = new PassthroughProperty<double>(backing);
passthrough.EventSynchronizationContext = SynchronizationContext.Current;
```

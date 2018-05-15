using System;
using System.Collections.Generic;

namespace KoKo.Property
{
    /// <summary>
    /// A property object whose single <see cref="Value"/> is calculated based off of other dependency properties.
    /// When any of the dependencies change their value, this property will automatically recalculate its <see cref="Value"/>.
    /// </summary>
    /// <typeparam name="T">The type of the property value returned by this property's calculation.</typeparam>
    /// See also <seealso cref="StoredProperty{T}"/>.
    public class DerivedProperty<T> : UnsettableProperty<T>
    {
        private T cachedValue;
        private readonly Func<T> calculator;

        /// <summary>
        /// Create a new <see cref="DerivedProperty{T}"/> that depends on the given other <paramref name="dependencies"/>
        /// properties, and calculates its own <see cref="Value"/> using <paramref name="calculator"/>.
        /// Whenever one of the dependency values changes, <paramref name="calculator"/> will be re-run, this property's <see cref="Value"/>
        /// will change, and a <see cref="SettableProperty{T}.PropertyChanged"/> event will be fired.
        /// </summary>
        /// <param name="dependencies">All of the properties used in the <paramref name="calculator"/> should be listed here, so
        /// that this property can update its own <see cref="Value"/> when any of its dependency values change.</param>
        /// <param name="calculator">The function used to give this property its <see cref="Value"/>. Any inputs into this function should be
        /// listed in <paramref name="dependencies"/>.</param>
        /// <example>
        /// <code>
        /// var name = new StoredProperty{string}("Ben");
        /// var greeting = new DerivedProperty{string}(new[] { name }, () => $"Hello {name.Value}");
        /// Console.WriteLine(greeting); // Hello Ben
        /// </code>
        /// </example>
        public DerivedProperty(IEnumerable<Property> dependencies, Func<T> calculator)
        {
            this.calculator = calculator;
            ComputeValue();
            ListenForDependencyUpdates(dependencies);
        }

        /// <summary>
        /// Gets the most recently calculated value of this property.
        /// Reading this value does not trigger a calculation, it uses a cached value from the most recently dependency change.
        /// To set this value, one of the dependency values must change.
        /// </summary>
        public override T Value => cachedValue;

        private void ComputeValue()
        {
            T newValue = calculator();
            if (!Equals(cachedValue, newValue))
            {
                cachedValue = newValue;
                OnValueChanged();
            }
        }

        private void ListenForDependencyUpdates(IEnumerable<Property> dependencies)
        {
            foreach (Property dependency in dependencies)
            {
                dependency.PropertyChanged += (sender, args) => ComputeValue();
            }
        }

    }
}
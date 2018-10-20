using System;
using System.Collections.Generic;

namespace KoKo.Property {

    /// <summary>
    /// A property object whose single <see cref="Value"/> is calculated based off of other dependency properties.
    /// When any of the dependencies change their value, this property will automatically recalculate its <see cref="Value"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the property value returned by this property's calculation.</typeparam>
    /// See also <seealso cref="StoredProperty{T}"/>.
    public class DerivedProperty<TResult>: UnsettableProperty<TResult> {

        private readonly Func<TResult> calculatorRunner;

        /*
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
        */

        private DerivedProperty(IEnumerable<Property> dependencies, Func<TResult> calculatorRunner): base(calculatorRunner()) {
            this.calculatorRunner = calculatorRunner;
            ListenForDependencyUpdates(dependencies);
        }

        public static DerivedProperty<TResult> Create<T1>(Property<T1> dependency1, Func<T1, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] { dependency1 }, () => calculator(dependency1.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2>(Property<T1> dependency1, Property<T2> dependency2, Func<T1, T2, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] { dependency1, dependency2 }, () => calculator(dependency1.Value, dependency2.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3, Func<T1, T2, T3, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] { dependency1, dependency2, dependency3 }, () => calculator(dependency1.Value, dependency2.Value, dependency3.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3, Property<T4> dependency4,
            Func<T1, T2, T3, T4, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] { dependency1, dependency2, dependency3, dependency4 },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3, Property<T4> dependency4,
            Property<T5> dependency5, Func<T1, T2, T3, T4, T5, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] { dependency1, dependency2, dependency3, dependency4, dependency5 },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3, Property<T4> dependency4,
            Property<T5> dependency5, Property<T6> dependency6, Func<T1, T2, T3, T4, T5, T6, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] { dependency1, dependency2, dependency3, dependency4, dependency5, dependency6 },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3, Property<T4> dependency4,
            Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] { dependency1, dependency2, dependency3, dependency4, dependency5, dependency6, dependency7 },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3, Property<T4> dependency4,
            Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Property<T8> dependency8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] { dependency1, dependency2, dependency3, dependency4, dependency5, dependency6, dependency7, dependency8 },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value, dependency8.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3, Property<T4> dependency4,
            Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Property<T8> dependency8, Property<T9> dependency9,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] { dependency1, dependency2, dependency3, dependency4, dependency5, dependency6, dependency7, dependency8, dependency9 },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value, dependency8.Value,
                    dependency9.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3,
            Property<T4> dependency4, Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Property<T8> dependency8, Property<T9> dependency9, Property<T10> dependency10,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> calculator) {
            return new DerivedProperty<TResult>(
                new Property[] { dependency1, dependency2, dependency3, dependency4, dependency5, dependency6, dependency7, dependency8, dependency9, dependency10 },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value, dependency8.Value,
                    dependency9.Value, dependency10.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3,
            Property<T4> dependency4, Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Property<T8> dependency8, Property<T9> dependency9, Property<T10> dependency10,
            Property<T11> dependency11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> calculator) {
            return new DerivedProperty<TResult>(
                new Property[] { dependency1, dependency2, dependency3, dependency4, dependency5, dependency6, dependency7, dependency8, dependency9, dependency10, dependency11 },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value, dependency8.Value,
                    dependency9.Value, dependency10.Value, dependency11.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3,
            Property<T4> dependency4, Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Property<T8> dependency8, Property<T9> dependency9, Property<T10> dependency10,
            Property<T11> dependency11, Property<T12> dependency12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> calculator) {
            return new DerivedProperty<TResult>(
                new Property[] { dependency1, dependency2, dependency3, dependency4, dependency5, dependency6, dependency7, dependency8, dependency9, dependency10, dependency11, dependency12 },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value, dependency8.Value,
                    dependency9.Value, dependency10.Value, dependency11.Value, dependency12.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3,
            Property<T4> dependency4, Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Property<T8> dependency8, Property<T9> dependency9, Property<T10> dependency10,
            Property<T11> dependency11, Property<T12> dependency12, Property<T13> dependency13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] {
                    dependency1,
                    dependency2,
                    dependency3,
                    dependency4,
                    dependency5,
                    dependency6,
                    dependency7,
                    dependency8,
                    dependency9,
                    dependency10,
                    dependency11,
                    dependency12,
                    dependency13
                },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value, dependency8.Value,
                    dependency9.Value, dependency10.Value, dependency11.Value, dependency12.Value, dependency13.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Property<T1> dependency1, Property<T2> dependency2, Property<T3> dependency3,
            Property<T4> dependency4, Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Property<T8> dependency8, Property<T9> dependency9, Property<T10> dependency10,
            Property<T11> dependency11, Property<T12> dependency12, Property<T13> dependency13, Property<T14> dependency14,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] {
                    dependency1,
                    dependency2,
                    dependency3,
                    dependency4,
                    dependency5,
                    dependency6,
                    dependency7,
                    dependency8,
                    dependency9,
                    dependency10,
                    dependency11,
                    dependency12,
                    dependency13,
                    dependency14
                },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value, dependency8.Value,
                    dependency9.Value, dependency10.Value, dependency11.Value, dependency12.Value, dependency13.Value, dependency14.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Property<T1> dependency1, Property<T2> dependency2,
            Property<T3> dependency3, Property<T4> dependency4, Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Property<T8> dependency8, Property<T9> dependency9,
            Property<T10> dependency10, Property<T11> dependency11, Property<T12> dependency12, Property<T13> dependency13, Property<T14> dependency14, Property<T15> dependency15,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] {
                    dependency1,
                    dependency2,
                    dependency3,
                    dependency4,
                    dependency5,
                    dependency6,
                    dependency7,
                    dependency8,
                    dependency9,
                    dependency10,
                    dependency11,
                    dependency12,
                    dependency13,
                    dependency14,
                    dependency15
                },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value, dependency8.Value,
                    dependency9.Value, dependency10.Value, dependency11.Value, dependency12.Value, dependency13.Value, dependency14.Value, dependency15.Value));
        }

        public static DerivedProperty<TResult> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Property<T1> dependency1, Property<T2> dependency2,
            Property<T3> dependency3, Property<T4> dependency4, Property<T5> dependency5, Property<T6> dependency6, Property<T7> dependency7, Property<T8> dependency8, Property<T9> dependency9,
            Property<T10> dependency10, Property<T11> dependency11, Property<T12> dependency12, Property<T13> dependency13, Property<T14> dependency14, Property<T15> dependency15,
            Property<T16> dependency16, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> calculator) {
            return new DerivedProperty<TResult>(new Property[] {
                    dependency1,
                    dependency2,
                    dependency3,
                    dependency4,
                    dependency5,
                    dependency6,
                    dependency7,
                    dependency8,
                    dependency9,
                    dependency10,
                    dependency11,
                    dependency12,
                    dependency13,
                    dependency14,
                    dependency15,
                    dependency16
                },
                () => calculator(dependency1.Value, dependency2.Value, dependency3.Value, dependency4.Value, dependency5.Value, dependency6.Value, dependency7.Value, dependency8.Value,
                    dependency9.Value, dependency10.Value, dependency11.Value, dependency12.Value, dependency13.Value, dependency14.Value, dependency15.Value, dependency16.Value));
        }

        /// <summary>
        /// Gets the most recently calculated value of this property.
        /// Reading this value does not trigger a calculation, it uses a cached value from the most recently dependency change.
        /// To set this value, one of the dependency values must change.
        /// </summary>
        public override TResult Value => CachedValue;

        protected override TResult ComputeValue() {
            return calculatorRunner();
        }

    }

}
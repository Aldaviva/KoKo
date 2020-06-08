using System;

namespace KoKo.Property {

    /// <summary>
    /// A property whose value is calculated using a provided function. The value is only calculated, and change events may only be
    /// fired, when you call <c>Recalculate()</c>. Otherwise, getting the value of this property will return the most-recently
    /// calculated value.
    /// </summary>
    /// <typeparam name="T">The type of this property's value, whic is also the type returned by the calculator</typeparam>
    public class ManuallyRecalculatedProperty<T>: UnsettableProperty<T> {

        private readonly Func<T> calculator;

        public override T Value => CachedValue;

        /// <summary>
        /// Create a new property that uses the specified <c>calculator</c> function to get the property's value. The calculator is run
        /// once when the property is constructed to get its initial value. Further value updates must be triggered manually by running
        /// <c>Recalculate()</c>.
        /// </summary>
        /// <param name="calculator">A function that returns the value of the property</param>
        public ManuallyRecalculatedProperty(Func<T> calculator): base(calculator()) {
            this.calculator = calculator;
        }

        protected override T ComputeValue() {
            return calculator();
        }

        /// <summary>
        /// Re-run the calculator to get the new value of this property. This is the only way for the value to change after creation.
        /// Like other Property classes, change events will be fired if and only if the new value is different from the most-recently
        /// calculated value.
        /// </summary>
        public void Recalculate() {
            ComputeValueAndFireChangeEvents();
        }

    }

}
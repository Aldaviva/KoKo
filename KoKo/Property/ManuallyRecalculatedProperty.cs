using System;

namespace KoKo.Property {

    public interface ManuallyRecalculatedProperty {

        /// <summary>
        /// Re-run the calculator to get the new value of this property. This is the only way for the value to change after creation.
        /// Like other Property classes, change events will be fired if and only if the new value is different from the most-recently
        /// calculated value.
        /// </summary>
        void Recalculate();

    }

    /// <summary>
    /// A property whose value is calculated using a provided function. The value is only calculated, and change events may only be
    /// fired, when you call <c>Recalculate()</c>. Otherwise, getting the value of this property will return the most-recently
    /// calculated value.
    /// </summary>
    /// <typeparam name="T">The type of this property's value, which is also the type returned by the calculator</typeparam>
    public class ManuallyRecalculatedProperty<T>: UnsettableProperty<T>, ManuallyRecalculatedProperty {

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

        /// <summary>
        /// <para>Constructor to be used when subclassing <see cref="ManuallyRecalculatedProperty{T}"/>, if you want to override
        /// <see cref="ComputeValue"/> instead of passing a <c>calculator</c> function to the
        /// <c>ManuallyRecalculatedProperty(Func&lt;T&gt;)</c> constructor.</para>
        /// <para>Unless you are subclassing <see cref="ManuallyRecalculatedProperty{T}"/>, you should instead call
        /// <c>new ManuallyRecalculatedProperty(Func&lt;T&gt; calculator)</c>.</para>
        /// </summary>
        /// <exception cref="NotImplementedException">if you call this constructor without subclassing <see cref="ManuallyRecalculatedProperty{T}"/> and overriding <see cref="ComputeValue"/></exception>
        protected ManuallyRecalculatedProperty(): base(default!) {
            calculator = () => throw new NotImplementedException("Please either pass a calculator function to the " +
                $"{nameof(ManuallyRecalculatedProperty)}(Func) constructor, or override its {nameof(ComputeValue)}() method");
            CachedValue = ComputeValue();
        }

        protected override T ComputeValue() {
            return calculator();
        }

        public void Recalculate() {
            ComputeValueAndFireChangeEvents();
        }

    }

}
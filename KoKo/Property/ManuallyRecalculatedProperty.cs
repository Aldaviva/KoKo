using System;

namespace KoKo.Property {

    public class ManuallyRecalculatedProperty<T>: UnsettableProperty<T> {

        private readonly Func<T> calculator;

        public override T Value => CachedValue;

        public ManuallyRecalculatedProperty(Func<T> calculator): base(calculator()) {
            this.calculator = calculator;
        }

        protected override T ComputeValue() {
            return calculator();
        }

        public void Recalculate() {
            ComputeValueAndFireChangeEvents();
        }

    }

}
using System;
using System.Collections.Generic;

namespace CoCo.Property
{
    public class DerivedProperty<T> : UnsettableProperty<T>
    {
        private T cachedValue;
        private readonly Func<T> calculator;

        public DerivedProperty(IEnumerable<Property> dependencies, Func<T> calculator)
        {
            this.calculator = calculator;
            ComputeValue();
            ListenForDependencyUpdates(dependencies);
        }

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

        public override T Value => cachedValue;
    }
}
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using KoKo.Events;

namespace KoKo.Property {

    /// <summary>
    /// A property object that cannot have its <see cref="Value"/> set directly, and can only change its <see cref="Value"/>
    /// based on some external trigger.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> exposed by this property.</typeparam>
    /// See also <seealso cref="DerivedProperty{T}"/>.
    public abstract class UnsettableProperty<T>: Property<T> {

        private readonly object cachedValueLock = new object();
        private readonly PropertyHelper<T> helper = new PropertyHelper<T>();

        protected T CachedValue;

        public abstract T Value { get; }

        object? Property.Value => Value;

        /// <summary>
        /// Specify an alternate threading model for running event handlers, instead of the thread that updated the Property's value.
        /// <para>
        /// By default, all KoKo Properties fire events synchronously on the thread that changed the value.
        /// This may not always be what you want, for example, if the Property is in a business logic class and is updated on a background thread, but the Property is also bound
        /// in a user interface control using Windows Forms or WPF. In this case, the update on the background thread would lead to a binding update of the UI control also on the
        /// background thread, which is illegal, since UI controls can only be updated on the main thread. To handle this case, you can specify a
        /// <see cref="SynchronizationContext"/> (typically <c>SynchronizationContext.Current</c>), and the <see cref="PropertyChanged"/> events will be run synchronously in that
        /// context instead (such as the WPF Dispatcher).</para>
        /// <para>Note that this will affect all event handlers you have registered for the Property instance, so if you just want to make your UI work properly, you may want to
        /// create a <see cref="PassthroughProperty{T}"/> in your view model with <c>SynchronizationContext.Current</c>, rather than changing the SynchronizationContext of your
        /// business logic model property.</para>
        /// </summary>
        public SynchronizationContext? EventSynchronizationContext {
            get => helper.EventSynchronizationContext;
            set => helper.EventSynchronizationContext = value;
        }

        protected UnsettableProperty(T initialValue) {
            CachedValue = initialValue;
        }

        protected abstract T ComputeValue();

        protected void ComputeValueAndFireChangeEvents(object? sender = null, PropertyChangedEventArgs? propertyChangedEventArgs = null) {
            T oldValue, newValue = ComputeValue();
            bool didValueChange;

            // Can't use wait-free Interlocked.Exchange<T>(T, T) because T isn't guaranteed to be a reference type, it might be an int or other value type, so use a lock to ensure we are comparing a consistent snapshot of the old and new values
            lock (cachedValueLock) {
                oldValue       = CachedValue;
                didValueChange = !Equals(oldValue, newValue);
                if (didValueChange) {
                    CachedValue = newValue;
                }
            }

            if (didValueChange) {
                OnValueChanged(oldValue, newValue);
            }
        }

        protected void ListenForDependencyUpdates(IEnumerable<Property> dependencies) {
            foreach (Property dependency in dependencies) {
                dependency.PropertyChanged += ComputeValueAndFireChangeEvents;
            }
        }

        internal void OnValueChanged(T oldValue, T newValue) {
            helper.OnValueChanged(this, oldValue, newValue);
        }

        public event KoKoPropertyChangedEventHandler<T> PropertyChanged {
            add => helper.PropertyChanged += value;
            remove => helper.PropertyChanged -= value;
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add => ((INotifyPropertyChanged) helper).PropertyChanged += value;
            remove => ((INotifyPropertyChanged) helper).PropertyChanged -= value;
        }

    }

}
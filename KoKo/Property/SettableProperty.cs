using System.ComponentModel;
using System.Threading;
using KoKo.Events;

namespace KoKo.Property {

    /// <summary>
    /// A property object that can have its <see cref="Value"/> set directly.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> exposed by this property.</typeparam>
    /// See also <seealso cref="StoredProperty{T}"/>.
    public abstract class SettableProperty<T>: Property<T> {

        private readonly PropertyHelper<T> helper = new PropertyHelper<T>();

        /// <summary>
        /// A gettable and settable property value.
        /// </summary>
        public abstract T Value { get; set; }

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

        object? Property.Value => Value;

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
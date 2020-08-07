using System.Threading;
using KoKo.Events;

namespace KoKo.Property {

    /// <summary>
    /// A passthrough property that takes an existing parent property and ensures that all of its change events are run asynchronously in the specified Synchronization Context. This lets you update
    /// the parent property on a background thread, then create a SynchronizationContextProperty in your user interface so that event handlers and view binding changes are run on the UI thread.
    /// Without this, the binding would try to update the UI on a background thread, which is disallowed by both Windows Forms and WPF.
    /// <para>Be aware that most KoKo OnPropertyChanged events are run synchronously on the thread that sets the property value, but this class is different in that the events are run asynchronously.
    /// This can lead to unpredictable behavior as properties update.</para>
    /// </summary>
    /// <typeparam name="T">The type of the parent property's value.</typeparam>
    public class SynchronizationContextProperty<T>: PassthroughProperty<T> {

        private readonly SynchronizationContext synchronizationContext;

        /// <summary>
        /// Create a new property that passes through the value of its <c>parentProperty</c>, and any event handlers or UI bindings you create will be run asynchronously in the specified
        /// <c>synchronizationContext</c> instead of the thread on which the parent value was originally updated.
        /// <para>Be aware that most KoKo OnPropertyChanged events are run synchronously on the thread that sets the property value, but this class is different in that the events are run
        /// asynchronously. This can lead to unpredictable behavior as properties update.</para>
        /// </summary>
        /// <param name="parentProperty">An existing property that may have its value set on a background thread.</param>
        /// <param name="synchronizationContext">Usually <c>SynchronizationContext.Current</c>. Make sure you evaluate this on the main thread, not a background thread.</param>
        public SynchronizationContextProperty(Property<T> parentProperty, SynchronizationContext synchronizationContext): base(parentProperty) {
            this.synchronizationContext = synchronizationContext;
        }

        protected override void OnParentPropertyChanged(object sender, KoKoPropertyChangedEventArgs<T> args) {
            synchronizationContext.Post(state => {
                base.OnParentPropertyChanged(sender, args);
            }, null);
        }

    }

}
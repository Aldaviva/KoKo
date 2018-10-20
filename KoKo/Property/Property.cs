using System.ComponentModel;
using KoKo.Events;

namespace KoKo.Property {

    // for use when you need a list of value-typed properties
    /// <summary>
    /// A non-generic property object, which is useful for making a list of dependencies for a <see cref="DerivedProperty{T}"/>.
    /// </summary>
    /// <inheritdoc cref="INotifyPropertyChanged"/>
    public interface Property: INotifyPropertyChanged {

        object Value { get; }

    }

    /// <summary>
    /// A generic property object that exposes a single <see cref="Value"/>.
    /// For concrete implementations you can create, see <seealso cref="StoredProperty{T}"/> and <seealso cref="DerivedProperty{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the exposed <see cref="Value"/></typeparam>
    /// <inheritdoc cref="INotifyPropertyChanged"/>
    public interface Property<T>: Property, KoKoNotifyPropertyChanged<T> {

        /// <summary>
        /// Returns the data stored in this instance.
        /// </summary>
        new T Value { get; }

    }

}
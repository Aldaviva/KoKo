using KoKo.Property;

namespace Test;

public class TestStoredProperty {

    [Fact]
    public void InitialValueReturned() {
        var property = new StoredProperty<int>(4);
        property.Value.Should().Be(4);
    }

    [Fact]
    public void UpdatedValueReturned() {
        var property = new StoredProperty<string>("hello");
        property.Value.Should().Be("hello");
        property.Value = "world";
        property.Value.Should().Be("world");
    }

    [Fact]
    public void EventsTriggered() {
        var property = new StoredProperty<double>(8.0);
        property.Value.Should().Be(8.0);

        int eventsTriggeredCount        = 0;
        int inotifyEventsTriggeredCount = 0;
        property.PropertyChanged += (_, _) => {
            eventsTriggeredCount++;
            property.Value.Should().Be(9.0);
        };
        ((Property) property).PropertyChanged += (_, _) => { inotifyEventsTriggeredCount++; };

        property.Value = 9.0;

        eventsTriggeredCount.Should().Be(1);
        inotifyEventsTriggeredCount.Should().Be(1);
    }

    [Fact]
    public void SetUnchangedValueDoesNotTriggerEvents() {
        var property = new StoredProperty<double>(8.0);
        property.Value.Should().Be(8.0);

        int eventsTriggeredCount = 0;
        property.PropertyChanged += (_, _) => {
            eventsTriggeredCount++;
            property.Value.Should().Be(8.0);
        };

        property.Value = 8.0;

        eventsTriggeredCount.Should().Be(0);
    }

    [Fact]
    public void ConstructingWithoutInitialValueUsesDefault() {
        new StoredProperty<string>().Value.Should().BeNull();
        new StoredProperty<bool>().Value.Should().BeFalse();
        new StoredProperty<int>().Value.Should().Be(0);
    }

    [Fact]
    public void GetObjectValueWithGenericCasting() {
        var storedProperty = new StoredProperty<int>();
        storedProperty.Value = 3;
        Property property = storedProperty;
        property.Value.Should().Be(3);
    }

    [Fact]
    public void UsesSynchronizationContextIfConfigured() {
        var stored = new StoredProperty<int>(1);

        int           eventsTriggeredCount   = 0;
        MySyncContext synchronizationContext = new();
        stored.EventSynchronizationContext =  synchronizationContext;
        stored.PropertyChanged             += (_, _) => { eventsTriggeredCount++; };

        synchronizationContext.PostCount.Should().Be(0);
        synchronizationContext.SendCount.Should().Be(0);

        stored.Value = 2;

        eventsTriggeredCount.Should().Be(1);
        synchronizationContext.SendCount.Should().Be(1);
        synchronizationContext.PostCount.Should().Be(0);
        stored.EventSynchronizationContext.Should().BeSameAs(synchronizationContext);
    }

    [Fact]
    public void CustomEqualityComparer() {
        var stored = new StoredProperty<double>(135.5) { EqualityComparer = new TolerantEqualityComparer(0.5) };
        stored.Value.Should().Be(135.5);

        int eventsTriggeredCount = 0;
        stored.PropertyChanged += (_, _) => { eventsTriggeredCount++; };

        stored.Value = 136.0;
        stored.Value.Should().Be(135.5);
        eventsTriggeredCount.Should().Be(0);

        stored.Value = 136.1;
        stored.Value.Should().Be(136.1);
        eventsTriggeredCount.Should().Be(1);
    }

    private class TolerantEqualityComparer(double tolerance): IEqualityComparer<double> {

        public bool Equals(double x, double y) => x - tolerance <= y && x >= y - tolerance;

        public int GetHashCode(double obj) => HashCode.Combine(obj);

    }

}
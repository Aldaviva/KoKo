using KoKo.Property;

namespace Test;

public class TestMultiLevelProperty {

    private readonly Participant                participant;
    private readonly Roster                     roster;
    private readonly MultiLevelProperty<string> currentParticipantName;
    private          int                        changeEventsFired;

    public TestMultiLevelProperty() {
        participant                     = new Participant();
        participant.Name.Value          = "A";
        roster                          = new Roster();
        roster.CurrentParticipant.Value = participant;

        currentParticipantName                 =  new MultiLevelProperty<string>(() => roster.CurrentParticipant.Value.Name);
        currentParticipantName.PropertyChanged += delegate { changeEventsFired++; };
    }

    [Fact]
    public void InitialValue() {
        currentParticipantName.Value.Should().Be("A");
        changeEventsFired.Should().Be(0, "property was never changed from its initial value");
    }

    [Fact]
    public void NewValueWhenChangingInnerProperty() {
        participant.Name.Value = "B";

        currentParticipantName.Value.Should().Be("B");
        changeEventsFired.Should().Be(1);
    }

    [Fact]
    public void NewValueWhenChangingOuterProperty() {
        Participant participant2 = new();
        participant2.Name.Value         = "C";
        roster.CurrentParticipant.Value = participant2;
        currentParticipantName.Value.Should().Be("C");
        changeEventsFired.Should().Be(1);
    }

    [Fact]
    public void NoUpdateWhenChangingUnusedProperty() {
        Participant participant2 = new();
        participant2.Name.Value         = "C";
        roster.CurrentParticipant.Value = participant2;
        changeEventsFired.Should().Be(1);

        participant.Name.Value = "D";
        currentParticipantName.Value.Should().NotBe("D");
        changeEventsFired.Should().Be(1, "an unrelated property changed, so our MultiLevel property should not have fired any more events");
    }

}

internal class Roster {

    public StoredProperty<Participant> CurrentParticipant { get; } = new();

}

internal class Participant {

    public StoredProperty<string> Name { get; } = new();

}
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ClientApplications.Domain.Tests.Unit.Builders;
using FluentAssertions;

namespace ClientApplications.Domain.Tests.Unit;

public class CreateOfCoupleTests
{
    [Fact]
    public void Couple_client_must_be_tow_people()
    {
        var client = new TestClientBuilder()
            .BuildOfCouple();

        client.IsError.Should().BeTrue();

        client.FirstError.Description.Should().Be
            (Errors.General.Required("spouse").Description);
    }

    [Fact]
    public void Create_couple_of_client()
    {
        var client = new TestClientBuilder()
            .WithSpouse()
            .BuildOfCouple();

        client.IsError.Should().BeFalse();

        client.Value.GetSpouse().FirstName.Value.Should().Be(Constants.SpouseFirstName);
        client.Value.GetSpouse().LastName.Value.Should().Be(Constants.SpouseLastName);
        client.Value.ClientType.Should().Be(ClientType.Couple);
    }
}

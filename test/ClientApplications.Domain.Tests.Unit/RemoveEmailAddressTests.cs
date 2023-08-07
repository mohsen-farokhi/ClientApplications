using ClientApplications.Domain.Tests.Unit.Builders;
using FluentAssertions;

namespace ClientApplications.Domain.Tests.Unit;

// As a practitioner, I can remove client's email.
// The email should not be selected as a reminder.

public class RemoveEmailAddressTests
{
    [Fact]
    public void The_email_address_must_be_present_in_the_clients_emails()
    {
        var client = new TestClientBuilder()
            .BuildOfAdult();

        var result = client.Value.RemoveEmailAddress("xxxxx@gmail.com");

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(Errors.General.Not_Found_Code);
    }

    [Fact]
    public void Email_should_not_be_selected_as_a_reminder()
    {
        var client = new TestClientBuilder()
            .WithReminderEmail(Constants.ReminderEmail)
            .BuildOfAdult();

        var result = client.Value.RemoveEmailAddress(Constants.ReminderEmail);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Failure);

        result.FirstError.Description.Should().Be
            (Resources.Messages.Validations.EmailIsSetAsReminder);
    }

    [Fact]
    public void Delete_an_email()
    {
        var client = new TestClientBuilder()
            .WithConsentGivenEmail(Constants.ConsentGivenEmail)
            .BuildOfAdult();

        var result = client.Value.RemoveEmailAddress(Constants.ConsentGivenEmail);

        result.IsError.Should().BeFalse();

        client.Value.EmailAddresses.FirstOrDefault(c => c.Value == Constants.ConsentGivenEmail)
            .Should().BeNull();
    }
}

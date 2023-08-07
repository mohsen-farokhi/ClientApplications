using ClientApplications.Domain.Tests.Unit.Builders;
using FluentAssertions;

namespace ClientApplications.Domain.Tests.Unit;

// As a practitioner, I can set a client's email as a reminder.
// The email must be selected as consent given and can only be a reminder email.

public class SetEmailAsReminderTests
{
    [Fact]
    public void The_email_address_must_be_present_in_the_clients_emails()
    {
        var client = new TestClientBuilder()
            .BuildOfAdult();

        var result = client.Value.SetReminderEmail("xxxxx@gmail.com");

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(Errors.General.Not_Found_Code);
    }

    [Fact]
    public void The_email_address_must_contain_consent()
    {
        var client = new TestClientBuilder()
            .WithNoConsentGivenEmail(Constants.NoConsentGivenEmail)
            .BuildOfAdult();

        var result = client.Value.SetReminderEmail
            (Constants.NoConsentGivenEmail);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Failure);

        result.FirstError.Description.Should().Be
            (Resources.Messages.Validations.EmailMustContainConsent);
    }

    [Fact]
    public void Only_one_email_can_be_set_as_a_reminder()
    {
        var client = new TestClientBuilder()
            .WithConsentGivenEmail(Constants.ConsentGivenEmail)
            .WithReminderEmail(Constants.ReminderEmail)
            .BuildOfAdult();

        var result = client.Value.SetReminderEmail
            (Constants.ConsentGivenEmail);

        result.IsError.Should().BeFalse();

        client.Value.EmailAddresses.First(c => c.Value == Constants.ReminderEmail).IsReminderEmail
            .Should().BeFalse();

        client.Value.EmailAddresses.Single(c => c.IsReminderEmail).Value
            .Should().Be(Constants.ConsentGivenEmail);
    }

    [Fact]
    public void Select_a_clients_email_as_reminder()
    {
        var client = new TestClientBuilder()
            .WithConsentGivenEmail(Constants.ConsentGivenEmail)
            .BuildOfAdult();

        var result = client.Value.SetReminderEmail(Constants.ConsentGivenEmail);

        result.IsError.Should().BeFalse();

        var emailAddress =
            client.Value.EmailAddresses.SingleOrDefault(c => c.IsReminderEmail);

        emailAddress.Should().NotBeNull();
    }
}
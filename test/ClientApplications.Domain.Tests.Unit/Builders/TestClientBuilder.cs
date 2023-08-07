using ClientApplications.Domain.Model.Client;
using ClientApplications.Domain.Model.Client.Builders;
using ClientApplications.Domain.Model.Client.ValueObjects;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;

namespace ClientApplications.Domain.Tests.Unit.Builders;

internal class TestClientBuilder
{
    private readonly IClientBuilder _clientBuilder;
    private readonly List<Client> _minorContacts = new();
    private Client _spouse;

    public TestClientBuilder()
    {
        _clientBuilder = ClientFactory
            .NewClient()
            .WithName(firstName: "Mohsen", lastName: "Farokhi")
            .WithBillingType(BillingType.SelfPay.Value);
    }

    public TestClientBuilder WithConsentGivenEmail
        (string emailAddress)
    {
        _clientBuilder.WithEmail
            (c => c.WithUrl(emailAddress)
                   .WithConsentGiven()
                   .Build());

        return this;
    }

    public TestClientBuilder WithNoConsentGivenEmail
        (string emailAddress)
    {
        _clientBuilder.WithEmail
            (c => c.WithUrl(emailAddress)
                   .Build());

        return this;
    }

    public TestClientBuilder WithLeaveTextMessagePhone
        (string phoneNumber)
    {
        _clientBuilder.WithPhoneNumber
            (c => c.WithNumber(phoneNumber)
                   .WithType(PhoneNumberType.Home.Value)
                   .WithLeaveTextMessage()
                   .Build());

        return this;
    }

    public TestClientBuilder WithLeaveVoiceMessagePhone
        (string phoneNumber)
    {
        _clientBuilder.WithPhoneNumber
            (c => c.WithNumber(phoneNumber)
                   .WithType(PhoneNumberType.Home.Value)
                   .WithLeaveVoiceMessage()
                   .Build());

        return this;
    }

    public TestClientBuilder WithNoLeaveTextMessagePhone
        (string phoneNumber)
    {
        _clientBuilder.WithPhoneNumber
            (c => c.WithNumber(phoneNumber)
                   .WithType(PhoneNumberType.Home.Value)
                   .Build());

        return this;
    }

    public TestClientBuilder WithNoLeaveVoiceMessagePhone
        (string phoneNumber)
    {
        _clientBuilder.WithPhoneNumber
            (c => c.WithNumber(phoneNumber)
                   .WithType(PhoneNumberType.Home.Value)
                   .Build());

        return this;
    }

    public TestClientBuilder WithReminderEmail
        (string emailAddress)
    {
        _clientBuilder.WithEmail(c =>
            c.WithUrl(emailAddress)
            .WithConsentGiven()
            .Build(), isReminder: true);

        return this;
    }

    public TestClientBuilder WithTextReminderPhoneNumber
        (string phoneNumber)
    {
        _clientBuilder.WithPhoneNumber
            (c => c.WithNumber(phoneNumber)
                   .WithType(PhoneNumberType.Home.Value)
                   .WithLeaveTextMessage()
                   .Build(), isTextReminder: true);

        return this;
    }

    public TestClientBuilder WithVoiceReminderPhoneNumber
        (string phoneNumber)
    {
        _clientBuilder.WithPhoneNumber
            (c => c.WithNumber(phoneNumber)
                   .WithType(PhoneNumberType.Home.Value)
                   .WithLeaveVoiceMessage()
                   .Build(), isVoiceReminder: true);

        return this;
    }

    public TestClientBuilder WithMinorRelation(long id)
    {
        var minorContact = new MinorContactBuilder()
            .WithName("Ali", "Farokhi")
            .WithRelationType(MinorRelationType.Parent.Value)
            .WithEmail(e => e
                .WithUrl("Ali@gmail.com")
                .WithType(EmailType.Home.Value)
                .WithConsentGiven()
                .Build(), isReminder: true)
            .WithPhoneNumber(p => p
                .WithNumber("09183659090")
                .WithType(PhoneNumberType.Mobile.Value)
                .WithLeaveTextMessage()
                .WithLeaveVoiceMessage()
                .Build(), isTextReminder: true, isVoiceReminder: true).Build().Value;

        minorContact?.GetType().GetProperty("Id")?
            .SetValue(minorContact, new ClientId(id));

        _minorContacts.Add(minorContact);

        return this;
    }

    public TestClientBuilder WithSpouse()
    {
        _spouse = new SpouseBuilder()
            .WithName(Constants.SpouseFirstName, Constants.SpouseLastName)
            .Build().Value;

        return this;
    }

    public ErrorOr<Client> BuildOfAdult() =>
        _clientBuilder.AsAdult().BuildOfAdult();

    public ErrorOr<Client> BuildOfMinor()
    {
        var client =
            _clientBuilder.AsMinor();

        if (_minorContacts.Any())
        {
            foreach (var contact in _minorContacts)
                client.WhichHasMinorContact(contact);
        }

        var minorResult = client
            .BuildOfMinor();

        if (minorResult.IsError)
            return minorResult;

        var minor = minorResult.Value;

        return minor;
    }

    public ErrorOr<Client> BuildOfCouple()
    {
        var client =
            _clientBuilder.AsCouple();

        if (_spouse != null)
            client.WhichHasSpouse(_spouse);

        var coupleResult = client
            .BuildOfCouple();

        if (coupleResult.IsError)
            return coupleResult;

        var couple = coupleResult.Value;

        return couple;
    }
}


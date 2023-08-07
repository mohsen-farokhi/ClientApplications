using ClientApplications.Domain.Model.Client.ValueObjects;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;

namespace ClientApplications.Domain.Model.Client.Builders;

public interface IClientBuilder
{
    IClientBuilder WithName
        (string firstName, string lastName);

    IClientBuilder WithEmail
        (Action<EmailAddressBuilder> builder, bool isReminder = false);

    IClientBuilder WithPhoneNumber
        (Action<PhoneNumberBuilder> builder,
         bool isTextReminder = false, bool isVoiceReminder = false);

    IClientBuilder WithBillingType
        (int billingTypeId);

    IAdultBuilder AsAdult();
    IMinorBuilder AsMinor();
    ICoupleBuilder AsCouple();
}

public interface IAdultBuilder
{
    ErrorOr<Client> BuildOfAdult();
}

public interface IMinorBuilder
{
    IMinorBuilder WhichHasMinorContact
        (Action<MinorContactBuilder> builder);

    IMinorBuilder WhichHasMinorContact
        (params Client[] contacts);

    ErrorOr<Client> BuildOfMinor();
}

public interface ICoupleBuilder
{
    ICoupleBuilder WhichHasSpouse
        (Action<SpouseBuilder> builder);

    ICoupleBuilder WhichHasSpouse
        (Client client);

    ErrorOr<Client> BuildOfCouple();
}

public class ClientBuilder :
    IClientBuilder,
    IAdultBuilder,
    IMinorBuilder,
    ICoupleBuilder
{
    private readonly List<Error> _errors = new();
    private readonly List<EmailAddress> _emailAddresses = new();
    private readonly List<PhoneNumber> _phoneNumbers = new();
    private FirstName _firstName;
    private LastName _lastName;
    private string _reminderEmailAddress;
    private string _reminderTextPhoneNumber;
    private string _reminderVoicePhoneNumber;
    private BillingType _billingType;
    private readonly List<Client> _minorContacts = new();
    private Client _spouse;

    public IClientBuilder WithName
        (string firstName, string lastName)
    {
        // **************************************************
        var firstNameResult = FirstName.Create(firstName);

        if (firstNameResult.IsError)
            _errors.AddRange(firstNameResult.Errors);
        else
            _firstName = firstNameResult.Value;
        // **************************************************

        // **************************************************
        var lastNameResult = LastName.Create(lastName);

        if (lastNameResult.IsError)
            _errors.AddRange(firstNameResult.Errors);
        else
            _lastName = lastNameResult.Value;
        // **************************************************

        return this;
    }

    public IClientBuilder WithEmail
        (Action<EmailAddressBuilder> builder, bool isReminder = false)
    {
        var emailAddressBuilder = new EmailAddressBuilder();

        builder.Invoke(emailAddressBuilder);

        var result = emailAddressBuilder.Build();

        if (result.IsError)
            _errors.AddRange(result.Errors);
        else
        {
            var email = result.Value;

            _emailAddresses.Add(email);

            if (isReminder)
                _reminderEmailAddress = email.Value;
        }

        return this;
    }

    public IClientBuilder WithPhoneNumber
        (Action<PhoneNumberBuilder> builder,
         bool isTextReminder = false, bool isVoiceReminder = false)
    {
        var phoneNumberBuilder = new PhoneNumberBuilder();

        builder.Invoke(phoneNumberBuilder);

        var result = phoneNumberBuilder.Build();

        if (result.IsError)
            _errors.AddRange(result.Errors);
        else
        {
            var phoneNumber = result.Value;

            _phoneNumbers.Add(phoneNumber);

            if (isTextReminder)
                _reminderTextPhoneNumber = phoneNumber.Value;

            if (isVoiceReminder)
                _reminderVoicePhoneNumber = phoneNumber.Value;
        }

        return this;
    }

    public IClientBuilder WithBillingType(int billingTypeId)
    {
        var billingTypeResult = BillingType.GetByValue(billingTypeId);

        if (billingTypeResult.IsError)
            _errors.AddRange(billingTypeResult.Errors);
        else
            _billingType = billingTypeResult.Value;

        return this;
    }

    public IMinorBuilder WhichHasMinorContact
        (Action<MinorContactBuilder> builder)
    {
        var minorContactBuilder = new MinorContactBuilder();

        builder.Invoke(minorContactBuilder);

        var result = minorContactBuilder.Build();

        if (result.IsError)
            _errors.AddRange(result.Errors);
        else
            _minorContacts.Add(result.Value);

        return this;
    }

    public IMinorBuilder WhichHasMinorContact
        (params Client[] contacts)
    {
        _minorContacts.AddRange(contacts);

        return this;
    }

    public ICoupleBuilder WhichHasSpouse
        (Action<SpouseBuilder> builder)
    {
        var spouseBuilder = new SpouseBuilder();

        builder.Invoke(spouseBuilder);

        var result = spouseBuilder.Build();

        if (result.IsError)
            _errors.AddRange(result.Errors);
        else
            _spouse = result.Value;

        return this;
    }

    public ICoupleBuilder WhichHasSpouse
        (Client client)
    {
        _spouse = client;

        return this;
    }

    public IAdultBuilder AsAdult()
    {
        return this;
    }

    public IMinorBuilder AsMinor()
    {
        return this;
    }

    public ICoupleBuilder AsCouple()
    {
        return this;
    }

    public ErrorOr<Client> BuildOfAdult()
    {
        if (_errors.Any())
            return _errors;

        // **************************************************
        var clientResult = Client.CreateOfAdult
            (_firstName, _lastName, _billingType);

        if (clientResult.IsError)
            return clientResult;
        // **************************************************

        var client = clientResult.Value;

        AddEmailAddresses(client);

        AddPhoneNumbers(client);

        if (client.ClientType == ClientType.Minor &&
            client.Contacts.Any() == false)
        {
            _errors.Add(Error.Validation
                (description: Resources.Messages.Validations.MinorClientMustHaveAtLeastOneGuardian));
        }

        if (_errors.Any())
            return _errors;

        return client;
    }

    public ErrorOr<Client> BuildOfMinor()
    {
        if (_errors.Any())
            return _errors;

        // **************************************************
        var clientResult = Client.CreateOfMinor
            (_firstName, _lastName, _billingType, _minorContacts);

        if (clientResult.IsError)
            return clientResult;
        // **************************************************

        var client = clientResult.Value;

        AddEmailAddresses(client);

        AddPhoneNumbers(client);

        return client;
    }

    public ErrorOr<Client> BuildOfCouple()
    {
        if (_errors.Any())
            return _errors;

        // **************************************************
        var clientResult = Client.CreateOfCouple
            (_firstName, _lastName,
             billingResponsible: true, spouse: _spouse);

        if (clientResult.IsError)
            return clientResult;
        // **************************************************

        var client = clientResult.Value;

        AddEmailAddresses(client);

        AddPhoneNumbers(client);

        return client;
    }

    private void AddPhoneNumbers(Client client)
    {
        foreach (var phoneNumber in _phoneNumbers)
            client.AddPhoneNumber(phoneNumber);

        if (_reminderTextPhoneNumber != null)
        {
            var setTextReminderPhoneResult =
                client.SetTextReminderPhoneNumber(_reminderTextPhoneNumber);

            if (setTextReminderPhoneResult.IsError)
                _errors.AddRange(setTextReminderPhoneResult.Errors);
        }

        if (_reminderVoicePhoneNumber != null)
        {
            var setVoiceReminderPhoneResult =
                client.SetVoiceReminderPhoneNumber(_reminderVoicePhoneNumber);

            if (setVoiceReminderPhoneResult.IsError)
                _errors.AddRange(setVoiceReminderPhoneResult.Errors);
        }
    }

    private void AddEmailAddresses(Client client)
    {
        foreach (var emailAddress in _emailAddresses)
            client.AddEmailAddress(emailAddress);

        if (_reminderEmailAddress != null)
        {
            var setReminderEmailResult =
                client.SetReminderEmail(_reminderEmailAddress);

            if (setReminderEmailResult.IsError)
                _errors.AddRange(setReminderEmailResult.Errors);
        }
    }

}

public static class ClientFactory
{
    public static IClientBuilder NewClient()
    {
        return new ClientBuilder();
    }
}

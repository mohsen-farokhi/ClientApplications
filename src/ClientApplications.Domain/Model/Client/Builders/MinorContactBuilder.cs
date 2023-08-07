using ClientApplications.Domain.Model.Client.ValueObjects;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;

namespace ClientApplications.Domain.Model.Client.Builders;

public class MinorContactBuilder
{
    private FirstName _firstName;
    private LastName _lastName;
    private string _reminderEmailAddress;
    private string _reminderTextPhoneNumber;
    private string _reminderVoicePhoneNumber;
    private MinorRelationType _relationType;
    private readonly List<Error> _errors = new();
    private readonly List<EmailAddress> _emailAddresses = new();
    private readonly List<PhoneNumber> _phoneNumbers = new();

    public MinorContactBuilder WithName
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

    public MinorContactBuilder WithRelationType
        (int relationType)
    {
        // **************************************************
        var typeResult =
            MinorRelationType.GetByValue(relationType);

        if (typeResult.IsError)
            _errors.AddRange(typeResult.Errors);
        // **************************************************

        _relationType = typeResult.Value;

        return this;
    }

    public MinorContactBuilder WithEmail
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

    public MinorContactBuilder WithPhoneNumber
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

    public ErrorOr<Client> Build()
    {
        if (_errors.Any())
            return _errors;

        // **************************************************
        var clientResult = Client.CreateOfMinorContact
            (_firstName, _lastName, _relationType);

        if (clientResult.IsError)
            return clientResult;
        // **************************************************

        var client = clientResult.Value;

        foreach (var emailAddress in _emailAddresses)
            client.AddEmailAddress(emailAddress);

        if (_reminderEmailAddress != null)
            client.SetReminderEmail(_reminderEmailAddress);

        if (_reminderTextPhoneNumber != null)
            client.SetTextReminderPhoneNumber(_reminderTextPhoneNumber);

        if (_reminderVoicePhoneNumber != null)
            client.SetVoiceReminderPhoneNumber(_reminderVoicePhoneNumber);

        return client;
    }
}

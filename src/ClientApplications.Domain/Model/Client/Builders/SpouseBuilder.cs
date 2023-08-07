using ClientApplications.Domain.Model.Client.ValueObjects;
using ErrorOr;

namespace ClientApplications.Domain.Model.Client.Builders;

public class SpouseBuilder
{
    private FirstName _firstName;
    private LastName _lastName;
    private string _reminderEmailAddress;
    private string _reminderTextPhoneNumber;
    private string _reminderVoicePhoneNumber;
    private bool _billingResponsible;
    private readonly List<Error> _errors = new();
    private readonly List<EmailAddress> _emailAddresses = new();
    private readonly List<PhoneNumber> _phoneNumbers = new();

    public SpouseBuilder WithName
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

    public SpouseBuilder WithEmail
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

    public SpouseBuilder WithPhoneNumber
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

    public SpouseBuilder WithBillingResponsible()
    {
        _billingResponsible = true;

        return this;
    }

    public ErrorOr<Client> Build()
    {
        if (_errors.Any())
            return _errors;

        // **************************************************
        var clientResult = Client.CreateOfSpouse
            (_firstName, _lastName, _billingResponsible);

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

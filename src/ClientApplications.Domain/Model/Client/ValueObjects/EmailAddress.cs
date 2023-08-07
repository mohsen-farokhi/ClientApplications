using ClientApplications.Domain.Model.Client.Exceptions;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;
using Framework.Domain;
using System.Net.Mail;

namespace ClientApplications.Domain.Model.Client.ValueObjects;

public class EmailAddress : ValueObject
{
    #region Constant(s)
    public const int MaxLength = 200;
    #endregion /Constant(s)

    internal static ErrorOr<EmailAddress> Create
        (string value, EmailType type, bool consentGiven)
    {
        // **************************************************
        value = Framework.Core.String.Fix(value);

        if (value is null)
            return Errors.General.Required(nameof(EmailAddress));

        if (value.Length > MaxLength)
            return Errors.General.MaxLength(nameof(EmailAddress), MaxLength);

        if (MailAddress.TryCreate(value, out var mailAddress) == false)
            return Errors.General.RegularExpression(nameof(EmailAddress));
        // **************************************************

        return new EmailAddress(value, type, consentGiven);
    }

    private EmailAddress
        (string value, EmailType type, bool consentGiven)
    {
        Value = value;
        Type = type;
        ConsentGiven = consentGiven;
    }

    private EmailAddress
        (string value, EmailType type,
         bool consentGiven, bool isReminder)
    {
        Value = value;
        Type = type;
        ConsentGiven = consentGiven;
        IsReminderEmail = isReminder;
    }

    private EmailAddress()
    {
    }

    public string Value { get; private set; }
    public EmailType Type { get; private set; }
    public bool ConsentGiven { get; private set; }
    public bool IsReminderEmail { get; private set; }

    internal EmailAddress SetAsConsentGiven()
    {
        var emailAddress = new EmailAddress
            (Value, Type, consentGiven: true);

        return emailAddress;
    }

    internal EmailAddress DisableConsent()
    {
        var canConsentBeDisabled = CanConsentBeDisabled();

        if (canConsentBeDisabled.IsError)
            throw new DisableConsentEmailException
                (canConsentBeDisabled.FirstError.Description);

        var emailAddress = new EmailAddress
            (Value, Type, consentGiven: false);

        return emailAddress;
    }

    internal EmailAddress SetAsReminder()
    {
        var canSetAsReminder = CanSetAsReminder();

        if (canSetAsReminder.IsError)
            throw new SetAsReminderEmailException
                (canSetAsReminder.FirstError.Description);

        var emailAddress = new EmailAddress
            (Value, Type, consentGiven: true, isReminder: true);

        return emailAddress;
    }

    internal EmailAddress DisableReminder()
    {
        var emailAddress = new EmailAddress
           (Value, Type, ConsentGiven, isReminder: false);

        return emailAddress;
    }

    internal ErrorOr<bool> CanConsentBeDisabled()
    {
        if (IsReminderEmail)
            return Error.Failure
                (description: Resources.Messages.Validations.EmailIsSetAsReminder);

        return true;
    }

    internal ErrorOr<bool> CanSetAsReminder()
    {
        if (ConsentGiven == false)
            return Error.Failure
                (description: Resources.Messages.Validations.EmailMustContainConsent);

        return true;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Type;
    }
}

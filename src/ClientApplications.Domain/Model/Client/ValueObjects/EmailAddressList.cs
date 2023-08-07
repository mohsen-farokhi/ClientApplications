using ErrorOr;
using Framework.Domain;
using System.Collections;

namespace ClientApplications.Domain.Model.Client.ValueObjects;

public class EmailAddressList : ValueObject, IEnumerable<EmailAddress>
{
    private readonly List<EmailAddress> _emailAddresses = new();
    public IReadOnlyCollection<EmailAddress> EmailAddresses => _emailAddresses;

    internal EmailAddressList()
    {
    }

    internal EmailAddressList(IEnumerable<EmailAddress> emailAddresses)
    {
        _emailAddresses = emailAddresses.ToList();
    }

    internal EmailAddressList AddEmailAddress(EmailAddress emailAddress)
    {
        var foundEmailAddress = _emailAddresses.FirstOrDefault
            (c => c.Value == emailAddress.Value);

        if (foundEmailAddress is null)
            _emailAddresses.Add(emailAddress);

        return new EmailAddressList(_emailAddresses);
    }

    internal ErrorOr<EmailAddressList> RemoveEmailAddress(string emailAddress)
    {
        // **************************************************
        var foundEmail = FindEmailAddress(emailAddress);

        if (foundEmail is null)
            return Errors.General.NotFound(nameof(EmailAddress));
        // **************************************************

        // **************************************************
        var canConsentBeDisabled =
            foundEmail.CanConsentBeDisabled();

        if (canConsentBeDisabled.IsError)
            return canConsentBeDisabled.FirstError;
        // **************************************************

        _emailAddresses.Remove(foundEmail);

        return new EmailAddressList(_emailAddresses);
    }

    internal ErrorOr<EmailAddressList> SetReminderEmail(string emailAddress)
    {
        // **************************************************
        var foundEmail = FindEmailAddress(emailAddress);

        if (foundEmail is null)
            return Errors.General.NotFound(nameof(EmailAddress));
        // **************************************************

        // **************************************************
        var canSetAsReminder =
            foundEmail.CanSetAsReminder();

        if (canSetAsReminder.IsError)
            return canSetAsReminder.FirstError;
        // **************************************************

        DisableReminderEmail();

        _emailAddresses.Remove(foundEmail);
        _emailAddresses.Add(foundEmail.SetAsReminder());

        return new EmailAddressList(_emailAddresses);
    }

    internal EmailAddressList DisableReminderEmail()
    {
        var reminderEmail =
            _emailAddresses.FirstOrDefault(c => c.IsReminderEmail);

        if (reminderEmail is not null)
        {
            _emailAddresses.Remove(reminderEmail);
            _emailAddresses.Add(reminderEmail.DisableReminder());
        }

        return new EmailAddressList(_emailAddresses);
    }

    internal ErrorOr<EmailAddressList> DisableConsentEmail(string emailAddress)
    {
        // **************************************************
        var foundEmail = FindEmailAddress(emailAddress);

        if (foundEmail is null)
            return Errors.General.NotFound(nameof(EmailAddress));
        // **************************************************

        // **************************************************
        var canConsentBeDisabled =
            foundEmail.CanConsentBeDisabled();

        if (canConsentBeDisabled.IsError)
            return canConsentBeDisabled.FirstError;
        // **************************************************

        _emailAddresses.Remove(foundEmail);
        _emailAddresses.Add(foundEmail.DisableConsent());

        return new EmailAddressList(_emailAddresses);
    }

    private EmailAddress? FindEmailAddress(string emailAddress)
    {
        emailAddress = Framework.Core.String.Fix(emailAddress);

        var foundEmail = _emailAddresses.FirstOrDefault
            (c => c.Value == emailAddress);

        return foundEmail;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        return _emailAddresses.OrderBy(x => x.Value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<EmailAddress> GetEnumerator()
    {
        return _emailAddresses.GetEnumerator();
    }
}

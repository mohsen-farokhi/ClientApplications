using ErrorOr;
using Framework.Domain;
using System.Collections;

namespace ClientApplications.Domain.Model.Client.ValueObjects;

public class PhoneNumberList : ValueObject, IEnumerable<PhoneNumber>
{
    private readonly List<PhoneNumber> _phoneNumbers = new();
    public IReadOnlyCollection<PhoneNumber> PhoneNumbers => _phoneNumbers;

    internal PhoneNumberList()
    {
    }

    internal PhoneNumberList(IEnumerable<PhoneNumber> phoneNumbers)
    {
        _phoneNumbers = phoneNumbers.ToList();
    }

    internal PhoneNumberList AddPhoneNumber(PhoneNumber phoneNumber)
    {
        var foundEmailAddress = _phoneNumbers.FirstOrDefault
            (c => c.Value == phoneNumber.Value);

        if (foundEmailAddress is null)
            _phoneNumbers.Add(phoneNumber);

        return new PhoneNumberList(_phoneNumbers);
    }

    internal ErrorOr<PhoneNumberList> RemovePhoneNumber(string phoneNumber)
    {
        // **************************************************
        var foundPhoneNumber = FindPhoneNumber(phoneNumber);

        if (foundPhoneNumber is null)
            return Errors.General.NotFound(nameof(PhoneNumber));
        // **************************************************

        // **************************************************
        var canLeaveTextBeDisabled =
            foundPhoneNumber.CanLeaveTextMessageDisabled();

        if (canLeaveTextBeDisabled.IsError)
            return canLeaveTextBeDisabled.FirstError;
        // **************************************************

        // **************************************************
        var canLeaveVoiceBeDisabled =
            foundPhoneNumber.CanLeaveVoiceMessageDisabled();

        if (canLeaveVoiceBeDisabled.IsError)
            return canLeaveVoiceBeDisabled.FirstError;
        // **************************************************

        _phoneNumbers.Remove(foundPhoneNumber);

        return new PhoneNumberList(_phoneNumbers);
    }

    internal ErrorOr<PhoneNumberList> SetTextReminderPhoneNumber(string phoneNumber)
    {
        // **************************************************
        var foundPhoneNumber = FindPhoneNumber(phoneNumber);

        if (foundPhoneNumber is null)
            return Errors.General.NotFound(nameof(PhoneNumber));
        // **************************************************

        // **************************************************
        var canSetAsReminder =
            foundPhoneNumber.CanSetAsTextReminder();

        if (canSetAsReminder.IsError)
            return canSetAsReminder.FirstError;
        // **************************************************

        DisableTextReminderPhoneNumber();

        _phoneNumbers.Remove(foundPhoneNumber);
        _phoneNumbers.Add(foundPhoneNumber.SetAsTextReminder());

        return new PhoneNumberList(_phoneNumbers);
    }

    internal ErrorOr<PhoneNumberList> SetVoiceReminderPhoneNumber(string phoneNumber)
    {
        // **************************************************
        var foundPhoneNumber = FindPhoneNumber(phoneNumber);

        if (foundPhoneNumber is null)
            return Errors.General.NotFound(nameof(PhoneNumber));
        // **************************************************

        // **************************************************
        var canSetAsReminder =
            foundPhoneNumber.CanSetAsVoiceReminder();

        if (canSetAsReminder.IsError)
            return canSetAsReminder.FirstError;
        // **************************************************

        DisableVoiceReminderPhoneNumber();

        _phoneNumbers.Remove(foundPhoneNumber);
        _phoneNumbers.Add(foundPhoneNumber.SetAsVoiceReminder());

        return new PhoneNumberList(_phoneNumbers);
    }

    internal PhoneNumberList DisableTextReminderPhoneNumber()
    {
        var textReminderNumber =
            _phoneNumbers.FirstOrDefault(c => c.IsTextReminder);

        if (textReminderNumber is not null)
        {
            _phoneNumbers.Remove(textReminderNumber);
            _phoneNumbers.Add(textReminderNumber.DisableTextReminder());
        }

        return new PhoneNumberList(_phoneNumbers);
    }

    internal PhoneNumberList DisableVoiceReminderPhoneNumber()
    {
        var voiceReminderNumber =
            _phoneNumbers.FirstOrDefault(c => c.IsVoiceReminder);

        if (voiceReminderNumber is not null)
        {
            _phoneNumbers.Remove(voiceReminderNumber);
            _phoneNumbers.Add(voiceReminderNumber.DisableVoiceReminder());
        }

        return new PhoneNumberList(_phoneNumbers);
    }

    internal ErrorOr<PhoneNumberList> DisableLeaveTextMessagePhoneNumber(string phoneNumber)
    {
        // **************************************************
        var foundPhoneNumber = FindPhoneNumber(phoneNumber);

        if (foundPhoneNumber is null)
            return Errors.General.NotFound(nameof(PhoneNumber));
        // **************************************************

        // **************************************************
        var canBeDisabled =
            foundPhoneNumber.CanLeaveTextMessageDisabled();

        if (canBeDisabled.IsError)
            return canBeDisabled.FirstError;
        // **************************************************

        _phoneNumbers.Remove(foundPhoneNumber);
        _phoneNumbers.Add(foundPhoneNumber.DisableLeaveTextMessage());

        return new PhoneNumberList(_phoneNumbers);
    }

    internal ErrorOr<PhoneNumberList> DisableLeaveVoiceMessagePhoneNumber(string phoneNumber)
    {
        // **************************************************
        var foundPhoneNumber = FindPhoneNumber(phoneNumber);

        if (foundPhoneNumber is null)
            return Errors.General.NotFound(nameof(PhoneNumber));
        // **************************************************

        // **************************************************
        var canBeDisabled =
            foundPhoneNumber.CanLeaveVoiceMessageDisabled();

        if (canBeDisabled.IsError)
            return canBeDisabled.FirstError;
        // **************************************************

        _phoneNumbers.Remove(foundPhoneNumber);
        _phoneNumbers.Add(foundPhoneNumber.DisableLeaveVoiceMessage());

        return new PhoneNumberList(_phoneNumbers);
    }

    private PhoneNumber? FindPhoneNumber
        (string phoneNumber)
    {
        phoneNumber = Framework.Core.String.Fix(phoneNumber);

        var foundPhoneNumber = _phoneNumbers.FirstOrDefault
            (c => c.Value == phoneNumber);

        return foundPhoneNumber;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        return _phoneNumbers.OrderBy(c => c.Value);
    }

    public IEnumerator<PhoneNumber> GetEnumerator()
    {
        return _phoneNumbers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

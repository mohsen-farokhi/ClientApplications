using ClientApplications.Domain.Model.Client.ValueObjects;
using ErrorOr;

namespace ClientApplications.Domain.Model.Client;

public partial class Client
{
    public PhoneNumberList PhoneNumbers { get; private set; } = new();

    public void AddPhoneNumber(PhoneNumber phoneNumber)
    {
        PhoneNumbers = PhoneNumbers.AddPhoneNumber(phoneNumber);
    }

    public ErrorOr<Success> RemovePhoneNumber(string phoneNumber)
    {
        var result =
            PhoneNumbers.RemovePhoneNumber(phoneNumber);

        if (result.IsError)
            return result.Errors;

        PhoneNumbers = result.Value;

        return Result.Success;
    }

    public ErrorOr<Success> SetTextReminderPhoneNumber(string phoneNumber)
    {
        var result =
            PhoneNumbers.SetTextReminderPhoneNumber(phoneNumber);

        if (result.IsError)
            return result.Errors;

        PhoneNumbers = result.Value;

        return Result.Success;
    }

    public ErrorOr<Success> SetVoiceReminderPhoneNumber(string phoneNumber)
    {
        var result =
            PhoneNumbers.SetVoiceReminderPhoneNumber(phoneNumber);

        if (result.IsError)
            return result.Errors;

        PhoneNumbers = result.Value;

        return Result.Success;
    }

    public void DisableTextReminderPhoneNumber()
    {
        PhoneNumbers = PhoneNumbers.DisableTextReminderPhoneNumber();
    }

    public void DisableVoiceReminderPhoneNumber()
    {
        PhoneNumbers = PhoneNumbers.DisableVoiceReminderPhoneNumber();
    }

    public ErrorOr<Success> DisableLeaveTextMessagePhoneNumber(string phoneNumber)
    {
        var result =
            PhoneNumbers.DisableLeaveTextMessagePhoneNumber(phoneNumber);

        if (result.IsError)
            return result.Errors;

        PhoneNumbers = result.Value;

        return Result.Success;
    }

    public ErrorOr<Success> DisableLeaveVoiceMessagePhoneNumber(string phoneNumber)
    {
        var result =
            PhoneNumbers.DisableLeaveVoiceMessagePhoneNumber(phoneNumber);

        if (result.IsError)
            return result.Errors;

        PhoneNumbers = result.Value;

        return Result.Success;
    }
}

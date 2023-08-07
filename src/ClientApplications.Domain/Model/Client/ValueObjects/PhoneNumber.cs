using ClientApplications.Domain.Model.Client.Exceptions;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects;

public class PhoneNumber : ValueObject
{
    #region Constant(s)
    public const int MaxLength = 11;
    #endregion /Constant(s)

    internal static ErrorOr<PhoneNumber> Create
        (string value, PhoneNumberType type,
         bool leaveTextMessage, bool leaveVoiceMessage)
    {
        // **************************************************
        value = Framework.Core.String.Fix(value);

        if (value is null)
            return Errors.General.Required(nameof(PhoneNumber));

        if (value.Length > MaxLength)
            return Errors.General.MaxLength(nameof(PhoneNumber), MaxLength);
        // **************************************************

        return new PhoneNumber
            (value, type, leaveTextMessage, leaveVoiceMessage);
    }

    private PhoneNumber
        (string value, PhoneNumberType type,
         bool leaveTextMessage, bool leaveVoiceMessage)
    {
        Value = value;
        Type = type;
        LeaveTextMessage = leaveTextMessage;
        LeaveVoiceMessage = leaveVoiceMessage;
    }

    private PhoneNumber
        (string value, PhoneNumberType type,
         bool leaveTextMessage, bool leaveVoiceMessage,
         bool isTextReminder, bool isVoiceReminder)
    {
        Value = value;
        Type = type;
        LeaveTextMessage = leaveTextMessage;
        LeaveVoiceMessage = leaveVoiceMessage;
        IsTextReminder = isTextReminder;
        IsVoiceReminder = isVoiceReminder;
    }

    private PhoneNumber()
    {
    }

    public string Value { get; private set; }
    public PhoneNumberType Type { get; private set; }
    public bool LeaveTextMessage { get; private set; }
    public bool LeaveVoiceMessage { get; private set; }
    public bool IsTextReminder { get; private set; }
    public bool IsVoiceReminder { get; private set; }

    internal PhoneNumber SetAsLeaveTextMessage()
    {
        return new PhoneNumber
            (Value, Type, leaveTextMessage: true, LeaveVoiceMessage);
    }

    internal PhoneNumber SetAsLeaveVoiceMessage()
    {
        return new PhoneNumber
            (Value, Type, LeaveTextMessage, leaveVoiceMessage: true);
    }

    internal PhoneNumber DisableLeaveTextMessage()
    {
        var canBeDisabled = CanLeaveTextMessageDisabled();

        if (canBeDisabled.IsError)
            throw new DisableLeaveTextMessageException
                (canBeDisabled.FirstError.Description);

        return new PhoneNumber
            (Value, Type, leaveTextMessage: false, LeaveVoiceMessage);
    }

    internal PhoneNumber DisableLeaveVoiceMessage()
    {
        var canBeDisabled = CanLeaveVoiceMessageDisabled();

        if (canBeDisabled.IsError)
            throw new DisableLeaveVoiceMessageException
                (canBeDisabled.FirstError.Description);

        return new PhoneNumber
            (Value, Type, LeaveTextMessage, leaveVoiceMessage: false);
    }

    internal PhoneNumber SetAsTextReminder()
    {
        var canSetAsReminder = CanSetAsTextReminder();

        if (canSetAsReminder.IsError)
            throw new SetAsTextReminderPhoneNumberException
                (canSetAsReminder.FirstError.Description);

        return new PhoneNumber
            (Value, Type, LeaveTextMessage, LeaveVoiceMessage,
             isTextReminder: true, IsVoiceReminder);
    }

    internal PhoneNumber SetAsVoiceReminder()
    {
        var canSetAsReminder = CanSetAsVoiceReminder();

        if (canSetAsReminder.IsError)
            throw new SetAsVoiceReminderPhoneNumberException
                (canSetAsReminder.FirstError.Description);

        return new PhoneNumber
            (Value, Type, LeaveTextMessage, LeaveVoiceMessage,
             IsTextReminder, isVoiceReminder: true);
    }

    internal PhoneNumber DisableTextReminder()
    {
        return new PhoneNumber
            (Value, Type, LeaveTextMessage, LeaveVoiceMessage,
             isTextReminder: false, IsVoiceReminder);
    }

    internal PhoneNumber DisableVoiceReminder()
    {
        return new PhoneNumber
            (Value, Type, LeaveTextMessage, LeaveVoiceMessage,
             IsTextReminder, isVoiceReminder: false);
    }

    internal ErrorOr<bool> CanLeaveTextMessageDisabled()
    {
        if (IsTextReminder)
            return Error.Failure
                (description: Resources.Messages.Validations.PhoneNumberIsSetAsTextReminder);

        return true;
    }

    internal ErrorOr<bool> CanLeaveVoiceMessageDisabled()
    {
        if (IsVoiceReminder)
            return Error.Failure
                (description: Resources.Messages.Validations.PhoneNumberIsSetAsVoiceReminder);

        return true;
    }

    internal ErrorOr<bool> CanSetAsTextReminder()
    {
        if (LeaveTextMessage == false)
            return Error.Failure
                (description: Resources.Messages.Validations.PhoneNumberMustSelectedAsTextMessageOk);

        return true;
    }

    internal ErrorOr<bool> CanSetAsVoiceReminder()
    {
        if (LeaveVoiceMessage == false)
            return Error.Failure
                (description: Resources.Messages.Validations.PhoneNumberMustSelectedAsVoiceMessageOk);

        return true;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Type;
    }
}

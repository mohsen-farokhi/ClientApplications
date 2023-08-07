using ClientApplications.Domain.Model.Client.ValueObjects;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;

namespace ClientApplications.Domain.Model.Client.Builders;

public class PhoneNumberBuilder
{
    private string _number;
    private PhoneNumberType _phoneNumberType;
    private bool _leaveTextMessage;
    private bool _leaveVoiceMessage;
    private readonly List<Error> _errors = new();

    public PhoneNumberBuilder WithType(int value)
    {
        var result = PhoneNumberType.GetByValue(value);

        if (result.IsError)
            _errors.AddRange(result.Errors);

        _phoneNumberType = result.Value;

        return this;
    }

    public PhoneNumberBuilder WithLeaveTextMessage()
    {
        _leaveTextMessage = true;

        return this;
    }

    public PhoneNumberBuilder WithLeaveVoiceMessage()
    {
        _leaveVoiceMessage = true;

        return this;
    }

    public PhoneNumberBuilder WithNumber(string value)
    {
        _number = value;

        return this;
    }

    public ErrorOr<PhoneNumber> Build()
    {
        if (_errors.Any())
            return _errors;

        var phoneNumberResult = PhoneNumber.Create
            (_number, _phoneNumberType, _leaveTextMessage, _leaveVoiceMessage);

        return phoneNumberResult;
    }
}

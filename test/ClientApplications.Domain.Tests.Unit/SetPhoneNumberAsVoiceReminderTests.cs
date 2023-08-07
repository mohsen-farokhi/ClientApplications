using ClientApplications.Domain.Tests.Unit.Builders;
using FluentAssertions;

namespace ClientApplications.Domain.Tests.Unit;

// As a practitioner, I can set a client's phone number as a voice reminder.
// The phone number must be selected as leave voice message and can only be a voice reminder phone number.

public class SetPhoneNumberAsVoiceReminderTests
{
    [Fact]
    public void The_phone_number_must_be_present_in_the_clients_phones()
    {
        var client = new TestClientBuilder()
            .BuildOfAdult();

        var result = client.Value.SetVoiceReminderPhoneNumber("09120000000");

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(Errors.General.Not_Found_Code);
    }

    [Fact]
    public void The_phone_number_must_contain_leave_voice_message()
    {
        var client = new TestClientBuilder()
            .WithNoLeaveVoiceMessagePhone(Constants.NoLeaveVoiceMessagePhoneNumber)
            .BuildOfAdult();

        var result = client.Value.SetVoiceReminderPhoneNumber
            (Constants.NoLeaveVoiceMessagePhoneNumber);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Failure);

        result.FirstError.Description.Should().Be
            (Resources.Messages.Validations.PhoneNumberMustSelectedAsVoiceMessageOk);
    }
}

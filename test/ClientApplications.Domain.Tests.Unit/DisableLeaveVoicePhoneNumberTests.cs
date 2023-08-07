using ClientApplications.Domain.Tests.Unit.Builders;
using FluentAssertions;

namespace ClientApplications.Domain.Tests.Unit;

// As a practitioner, I can disable client's leave voice phone number.
// The phone number should not be selected as a voice reminder.

public class DisableLeaveVoicePhoneNumberTests
{
    [Fact]
    public void The_phone_number_must_be_present_in_the_clients_phones()
    {
        var client = new TestClientBuilder()
            .BuildOfAdult();

        var result = client.Value.DisableLeaveVoiceMessagePhoneNumber("09120000000");

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(Errors.General.Not_Found_Code);
    }

    [Fact]
    public void Phone_number_should_not_be_selected_as_a_voice_reminder()
    {
        var client = new TestClientBuilder()
            .WithVoiceReminderPhoneNumber(Constants.VoiceReminderPhoneNumber)
            .BuildOfAdult();

        var result = client.Value.DisableLeaveVoiceMessagePhoneNumber
            (Constants.VoiceReminderPhoneNumber);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Failure);

        result.FirstError.Description.Should().Be
            (Resources.Messages.Validations.PhoneNumberIsSetAsVoiceReminder);
    }

    [Fact]
    public void Disable_a_leave_voice_phone_number()
    {
        var client = new TestClientBuilder()
            .WithLeaveVoiceMessagePhone(Constants.LeaveVoiceMessagePhoneNumber)
            .BuildOfAdult();

        var result = client.Value.DisableLeaveVoiceMessagePhoneNumber
            (Constants.LeaveVoiceMessagePhoneNumber);

        result.IsError.Should().BeFalse();

        var phoneNumber = client.Value.PhoneNumbers.First
            (c => c.Value == Constants.LeaveVoiceMessagePhoneNumber);

        phoneNumber.LeaveVoiceMessage.Should().BeFalse();
    }
}

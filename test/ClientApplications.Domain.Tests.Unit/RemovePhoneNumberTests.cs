using ClientApplications.Domain.Tests.Unit.Builders;
using FluentAssertions;

namespace ClientApplications.Domain.Tests.Unit;

// As a practitioner, I can remove client's phone number.
// The phone number should not be selected as a reminder.

public class RemovePhoneNumberTests
{
    [Fact]
    public void The_phone_number_must_be_present_in_the_clients_phones()
    {
        var client = new TestClientBuilder()
            .BuildOfAdult();

        var result = client.Value.RemovePhoneNumber("09120000000");

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(Errors.General.Not_Found_Code);
    }

    [Fact]
    public void Phone_number_should_not_be_selected_as_a_text_reminder()
    {
        var client = new TestClientBuilder()
            .WithTextReminderPhoneNumber(Constants.TextReminderPhoneNumber)
            .BuildOfAdult();

        var result = client.Value.RemovePhoneNumber(Constants.TextReminderPhoneNumber);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Failure);

        result.FirstError.Description.Should().Be
            (Resources.Messages.Validations.PhoneNumberIsSetAsTextReminder);
    }

    [Fact]
    public void Phone_number_should_not_be_selected_as_a_voice_reminder()
    {
        var client = new TestClientBuilder()
            .WithVoiceReminderPhoneNumber(Constants.VoiceReminderPhoneNumber)
            .BuildOfAdult();

        var result = client.Value.RemovePhoneNumber(Constants.VoiceReminderPhoneNumber);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Failure);

        result.FirstError.Description.Should().Be
            (Resources.Messages.Validations.PhoneNumberIsSetAsVoiceReminder);
    }

    [Fact]
    public void Delete_a_leave_text_phone_number()
    {
        var client = new TestClientBuilder()
            .WithLeaveTextMessagePhone(Constants.LeaveTextMessagePhoneNumber)
            .BuildOfAdult();

        var result = client.Value.RemovePhoneNumber(Constants.LeaveTextMessagePhoneNumber);

        result.IsError.Should().BeFalse();

        client.Value.PhoneNumbers.FirstOrDefault(c => c.Value == Constants.LeaveTextMessagePhoneNumber)
            .Should().BeNull();
    }

    [Fact]
    public void Delete_a_leave_voice_phone_number()
    {
        var client = new TestClientBuilder()
            .WithLeaveVoiceMessagePhone(Constants.LeaveVoiceMessagePhoneNumber)
            .BuildOfAdult();

        var result = client.Value.RemovePhoneNumber(Constants.LeaveVoiceMessagePhoneNumber);

        result.IsError.Should().BeFalse();

        client.Value.PhoneNumbers.FirstOrDefault(c => c.Value == Constants.LeaveVoiceMessagePhoneNumber)
            .Should().BeNull();
    }
}

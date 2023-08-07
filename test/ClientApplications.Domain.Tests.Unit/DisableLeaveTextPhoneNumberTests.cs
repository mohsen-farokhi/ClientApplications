using ClientApplications.Domain.Tests.Unit.Builders;
using FluentAssertions;

namespace ClientApplications.Domain.Tests.Unit;

// As a practitioner, I can disable client's leave text phone number.
// The phone number should not be selected as a text reminder.

public class DisableLeaveTextPhoneNumberTests
{
    [Fact]
    public void The_phone_number_must_be_present_in_the_clients_phones()
    {
        var client = new TestClientBuilder()
            .BuildOfAdult();

        var result = client.Value.DisableLeaveTextMessagePhoneNumber("09120000000");

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(Errors.General.Not_Found_Code);
    }

    [Fact]
    public void Phone_number_should_not_be_selected_as_a_text_reminder()
    {
        var client = new TestClientBuilder()
            .WithTextReminderPhoneNumber(Constants.TextReminderPhoneNumber)
            .BuildOfAdult();

        var result = client.Value.DisableLeaveTextMessagePhoneNumber
            (Constants.TextReminderPhoneNumber);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorOr.ErrorType.Failure);

        result.FirstError.Description.Should().Be
            (Resources.Messages.Validations.PhoneNumberIsSetAsTextReminder);
    }

    [Fact]
    public void Disable_a_leave_text_phone_number()
    {
        var client = new TestClientBuilder()
            .WithLeaveTextMessagePhone(Constants.LeaveTextMessagePhoneNumber)
            .BuildOfAdult();

        var result = client.Value.DisableLeaveTextMessagePhoneNumber
            (Constants.LeaveTextMessagePhoneNumber);

        result.IsError.Should().BeFalse();

        client.Value.PhoneNumbers.First(c => c.Value == Constants.LeaveTextMessagePhoneNumber)
            .LeaveTextMessage.Should().BeFalse();
    }
}

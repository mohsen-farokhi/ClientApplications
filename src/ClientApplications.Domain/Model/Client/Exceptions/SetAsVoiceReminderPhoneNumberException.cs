namespace ClientApplications.Domain.Model.Client.Exceptions;

public class SetAsVoiceReminderPhoneNumberException : Exception
{
    public SetAsVoiceReminderPhoneNumberException()
    {
    }

    public SetAsVoiceReminderPhoneNumberException(string message) : base(message)
    {
    }
}

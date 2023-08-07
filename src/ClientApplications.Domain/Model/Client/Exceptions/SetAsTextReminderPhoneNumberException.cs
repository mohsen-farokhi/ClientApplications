namespace ClientApplications.Domain.Model.Client.Exceptions;

public class SetAsTextReminderPhoneNumberException : Exception
{
    public SetAsTextReminderPhoneNumberException()
    {
    }

    public SetAsTextReminderPhoneNumberException(string message) : base(message)
    {
    }
}

namespace ClientApplications.Domain.Model.Client.Exceptions;

public class SetAsReminderEmailException : Exception
{
    public SetAsReminderEmailException()
    {
    }

    public SetAsReminderEmailException(string message) : base(message)
    {
    }
}

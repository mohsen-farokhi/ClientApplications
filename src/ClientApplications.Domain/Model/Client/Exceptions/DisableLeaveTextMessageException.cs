namespace ClientApplications.Domain.Model.Client.Exceptions;

public class DisableLeaveTextMessageException : Exception
{
    public DisableLeaveTextMessageException()
    {
    }

    public DisableLeaveTextMessageException(string message) : base(message)
    {
    }
}

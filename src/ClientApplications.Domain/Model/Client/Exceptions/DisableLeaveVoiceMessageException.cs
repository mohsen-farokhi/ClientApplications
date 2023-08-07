namespace ClientApplications.Domain.Model.Client.Exceptions;

public class DisableLeaveVoiceMessageException : Exception
{
    public DisableLeaveVoiceMessageException()
    {
    }

    public DisableLeaveVoiceMessageException(string message) : base(message)
    {
    }
}

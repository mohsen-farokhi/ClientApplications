namespace ClientApplications.Domain.Model.Client.Exceptions;

public class DisableConsentEmailException : Exception
{
    public DisableConsentEmailException()
    {
    }

    public DisableConsentEmailException(string message) : base(message)
    {
    }
}

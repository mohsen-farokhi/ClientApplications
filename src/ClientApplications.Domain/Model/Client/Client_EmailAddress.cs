using ClientApplications.Domain.Model.Client.ValueObjects;
using ErrorOr;

namespace ClientApplications.Domain.Model.Client;

public partial class Client
{
    public EmailAddressList EmailAddresses { get; private set; } = new();

    public void AddEmailAddress(EmailAddress emailAddress)
    {
        EmailAddresses = EmailAddresses.AddEmailAddress(emailAddress);
    }

    public ErrorOr<Success> RemoveEmailAddress(string emailAddress)
    {
        var result =
            EmailAddresses.RemoveEmailAddress(emailAddress);

        if (result.IsError)
            return result.Errors;

        EmailAddresses = result.Value;

        return Result.Success;
    }

    public ErrorOr<Success> SetReminderEmail(string emailAddress)
    {
        var result =
            EmailAddresses.SetReminderEmail(emailAddress);

        if (result.IsError)
            return result.Errors;

        EmailAddresses = result.Value;

        return Result.Success;
    }

    public void DisableReminderEmail()
    {
        EmailAddresses =
            EmailAddresses.DisableReminderEmail();
    }

    public ErrorOr<Success> DisableConsentEmail(string emailAddress)
    {
        var result =
            EmailAddresses.DisableConsentEmail(emailAddress);

        if (result.IsError)
            return result.Errors;

        EmailAddresses = result.Value;

        return Result.Success;
    }

}

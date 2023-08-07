using ClientApplications.Domain.Model.Client.ValueObjects;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;

namespace ClientApplications.Domain.Model.Client.Builders;

public class EmailAddressBuilder
{
    private string _url;
    private bool _consentGiven;
    private readonly EmailType _emailType;
    private readonly List<Error> _errors = new();

    public EmailAddressBuilder WithType(int value)
    {
        var result = EmailType.GetByValue(value);

        if (result.IsError)
            _errors.AddRange(result.Errors);

        return this;
    }

    public EmailAddressBuilder WithConsentGiven()
    {
        _consentGiven = true;

        return this;
    }

    public EmailAddressBuilder WithUrl(string url)
    {
        _url = url;

        return this;
    }

    public ErrorOr<EmailAddress> Build()
    {
        if (_errors.Any())
            return _errors;

        var emailAddressResult = EmailAddress.Create
            (_url, _emailType, _consentGiven);

        return emailAddressResult;
    }
}

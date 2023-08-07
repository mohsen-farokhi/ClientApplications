using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;

public class EmailType : Enumeration
{
    #region Constant(s)
    public const int MaxLength = 50;
    #endregion /Constant(s)

    #region Static Member(s)
    public static readonly EmailType Home = new(1, Resources.DataDictionary.Home);
    public static readonly EmailType Work = new(2, Resources.DataDictionary.Work);

    public static ErrorOr<EmailType> GetByValue(int? value)
    {
        if (value is null)
            return Errors.General.Required(nameof(EmailType));

        var emailType =
            FromValue<EmailType>(value: value.Value);

        if (emailType is null)
            return Errors.General.InvalidCode(nameof(EmailType));

        return emailType;
    }
    #endregion /Static Member(s)

    private EmailType(int value, string name) : base(value, name)
    {
    }

    private EmailType()
    {
    }
}

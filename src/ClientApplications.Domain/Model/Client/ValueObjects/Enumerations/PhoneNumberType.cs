using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;

public class PhoneNumberType : Enumeration
{
    #region Constant(s)
    public const int MaxLength = 50;
    #endregion /Constant(s)

    #region Static Member(s)
    public static readonly PhoneNumberType Mobile = new(1, Resources.DataDictionary.Mobile);
    public static readonly PhoneNumberType Work = new(2, Resources.DataDictionary.Work);
    public static readonly PhoneNumberType Home = new(3, Resources.DataDictionary.Home);
    public static readonly PhoneNumberType Fax = new(4, Resources.DataDictionary.Fax);

    public static ErrorOr<PhoneNumberType> GetByValue(int? value)
    {
        if (value is null)
            return Errors.General.Required(nameof(PhoneNumberType));

        var phoneNumberType =
            FromValue<PhoneNumberType>(value: value.Value);

        if (phoneNumberType is null)
            return Errors.General.InvalidCode(nameof(PhoneNumberType));

        return phoneNumberType;
    }
    #endregion /Static Member(s)

    private PhoneNumberType(int value, string name) : base(value, name)
    {
    }

    private PhoneNumberType()
    {
    }
}

using ClientApplications.Domain;
using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;

public class Gender : Enumeration
{
    #region Constant(s)
    public const int MaxLength = 50;
    #endregion /Constant(s)

    #region Static Member(s)
    public static readonly Gender Male = new(1, Resources.DataDictionary.Male);
    public static readonly Gender Female = new(2, Resources.DataDictionary.Female);

    public static ErrorOr<Gender> GetByValue(int? value)
    {
        if (value is null)
            return Errors.General.Required(nameof(Gender));

        var gender =
            FromValue<Gender>(value: value.Value);

        if (gender is null)
            return Errors.General.InvalidCode(nameof(Gender));

        return gender;
    }
    #endregion /Static Member(s)

    private Gender(int value, string name) : base(value, name)
    {
    }

    private Gender()
    {
    }
}

using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;

public class MinorRelationType : Enumeration
{
    #region Constant(s)
    public const int MaxLength = 50;
    #endregion /Constant(s)

    #region Static Member(s)
    public static readonly MinorRelationType Parent = new(1, Resources.DataDictionary.Parent);
    public static readonly MinorRelationType LegalGuardian = new(2, Resources.DataDictionary.LegalGuardian);
    public static readonly MinorRelationType FamilyMember = new(3, Resources.DataDictionary.FamilyMember);
    public static readonly MinorRelationType Other = new(4, Resources.DataDictionary.Other);

    public static ErrorOr<MinorRelationType> GetByValue(int? value)
    {
        if (value is null)
            return Errors.General.Required(nameof(MinorRelationType));

        var relationType =
            FromValue<MinorRelationType>(value: value.Value);

        if (relationType is null)
            return Errors.General.InvalidCode(nameof(MinorRelationType));

        return relationType;
    }
    #endregion /Static Member(s)

    private MinorRelationType(int value, string name) : base(value, name)
    {
    }

    private MinorRelationType()
    {
    }
}

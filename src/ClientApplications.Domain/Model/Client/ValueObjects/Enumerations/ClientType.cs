using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;

public class ClientType : Enumeration
{
    #region Constant(s)
    public const int MaxLength = 50;
    #endregion /Constant(s)

    #region Static Member(s)
    public static readonly ClientType Adult = new(1, Resources.DataDictionary.Adult);
    public static readonly ClientType Minor = new(2, Resources.DataDictionary.Minor);
    public static readonly ClientType Couple = new(3, Resources.DataDictionary.Couple);

    public static ErrorOr<ClientType> GetByValue(int? value)
    {
        if (value is null)
            return Errors.General.Required(nameof(ClientType));

        var clientType =
            FromValue<ClientType>(value: value.Value);

        if (clientType is null)
            return Errors.General.InvalidCode(nameof(ClientType));

        return clientType;
    }
    #endregion /Static Member(s)

    private ClientType(int value, string name) : base(value, name)
    {
    }

    private ClientType()
    {
    }
}

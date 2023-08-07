using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects;

public class LastName : ValueObject
{
    #region Constant(s)
    public const int MaxLength = 50;
    #endregion /Constant(s)

    public static ErrorOr<LastName> Create(string value)
    {
        value = Framework.Core.String.Fix(value);

        if (value is null)
            return Errors.General.Required(nameof(LastName));

        if (value.Length > MaxLength)
            return Errors.General.MaxLength(nameof(LastName), MaxLength);

        return new LastName(value);
    }

    private LastName() { }
    private LastName(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
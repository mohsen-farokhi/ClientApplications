using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects;

public class FirstName : ValueObject
{
    #region Constant(s)
    public const int MaxLength = 50;
    #endregion /Constant(s)

    public static ErrorOr<FirstName> Create(string value)
    {
        value = Framework.Core.String.Fix(value);

        if (value is null)
            return Errors.General.Required(nameof(FirstName));

        if (value.Length > MaxLength)
            return Errors.General.MaxLength(nameof(FirstName), MaxLength);

        return new FirstName(value);
    }

    private FirstName() { }
    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

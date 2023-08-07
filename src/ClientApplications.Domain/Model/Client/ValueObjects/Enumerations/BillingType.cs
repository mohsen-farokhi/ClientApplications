using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;

public class BillingType : Enumeration
{
    #region Constant(s)
    public const int MaxLength = 50;
    #endregion /Constant(s)

    #region Static Member(s)
    public static readonly BillingType SelfPay = new(1, Resources.DataDictionary.SelfPay);
    public static readonly BillingType Insurance = new(2, Resources.DataDictionary.Insurance);

    public static ErrorOr<BillingType> GetByValue(int? value)
    {
        if (value is null)
            return Errors.General.Required(nameof(BillingType));

        var billingType =
            FromValue<BillingType>(value: value.Value);

        if (billingType is null)
            return Errors.General.InvalidCode(nameof(BillingType));

        return billingType;
    }
    #endregion /Static Member(s)

    private BillingType(int value, string name) : base(value, name)
    {
    }

    private BillingType()
    {
    }
}

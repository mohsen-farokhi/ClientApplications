using ClientApplications.Domain.Model.Client.ValueObjects;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;

namespace ClientApplications.Domain.Model.Client;

public partial class Client
{
    internal static ErrorOr<Client> CreateOfCouple
        (FirstName legalFirstName, LastName legalLastName,
         bool billingResponsible, Client spouse)
    {
        var errors = new List<Error>();

        if (legalFirstName is null)
            errors.Add(Errors.General.Required(nameof(FirstName)));

        if (legalLastName is null)
            errors.Add(Errors.General.Required(nameof(LastName)));

        if (spouse is null)
            errors.Add(Errors.General.Required(nameof(spouse)));
        else if (spouse.BillingResponsible)
            billingResponsible = false;

        if (errors.Any())
            return errors;

        return new Client
            (legalFirstName, legalLastName,
             billingResponsible, spouse);
    }

    internal static ErrorOr<Client> CreateOfSpouse
        (FirstName legalFirstName, LastName legalLastName,
         bool billingResponsible)
    {
        var errors = new List<Error>();

        if (legalFirstName is null)
            errors.Add(Errors.General.Required(nameof(FirstName)));

        if (legalLastName is null)
            errors.Add(Errors.General.Required(nameof(LastName)));

        if (errors.Any())
            return errors;

        return new Client
            (legalFirstName, legalLastName,
             ClientType.Couple, billingResponsible);
    }

    private Client
        (FirstName firstName, LastName lastName,
         bool billingResponsible, Client spouse)
    {
        FirstName = firstName;
        LastName = lastName;
        BillingResponsible = billingResponsible;
        ClientType = ClientType.Couple;
        _contacts.Add(spouse);
        IsActive = true;
    }

    private Client
        (FirstName firstName, LastName lastName,
         ClientType clientType, bool billingResponsible)
    {
        FirstName = firstName;
        LastName = lastName;
        BillingResponsible = billingResponsible;
        ClientType = clientType;
        IsActive = true;
    }

    public bool BillingResponsible { get; private set; }

    public Client? GetSpouse() =>
        _contacts.FirstOrDefault();
}

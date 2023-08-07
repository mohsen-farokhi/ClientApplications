using ClientApplications.Domain.Model.Client.Exceptions;
using ClientApplications.Domain.Model.Client.ValueObjects;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;
namespace ClientApplications.Domain.Model.Client;

public partial class Client
{
    internal static ErrorOr<Client> CreateOfMinor
        (FirstName firstName, LastName lastName,
         BillingType billingType, IList<Client> contacts)
    {
        // **************************************************
        var requirementsCheckResult = CheckRequirements
            (firstName, lastName, billingType);

        if (requirementsCheckResult.IsError)
            return requirementsCheckResult.Errors;
        // **************************************************

        if (contacts == null || contacts.Any() == false)
            return Error.Validation(description:
                Resources.Messages.Validations.MinorClientMustHaveAtLeastOneGuardian);

        return new Client
            (firstName, lastName, billingType,
             ClientType.Minor, contacts);
    }

    internal static ErrorOr<Client> CreateOfMinorContact
        (FirstName firstName, LastName lastName,
         MinorRelationType relationType)
    {
        var errors = new List<Error>();

        if (firstName is null)
            errors.Add(Errors.General.Required(nameof(FirstName)));

        if (lastName is null)
            errors.Add(Errors.General.Required(nameof(LastName)));

        if (relationType is null)
            errors.Add(Errors.General.Required(nameof(MinorRelationType)));

        if (errors.Any())
            return errors;

        return new Client
            (firstName, lastName, relationType);
    }

    private Client
        (FirstName firstName, LastName lastName, BillingType billingType,
         ClientType clientType, IList<Client> contacts)
    {
        FirstName = firstName;
        LastName = lastName;
        ClientType = clientType;
        BillingType = billingType;
        _contacts = contacts.ToList();
    }

    private Client
        (FirstName firstName, LastName lastName,
         MinorRelationType minorRelationType)
    {
        FirstName = firstName;
        LastName = lastName;
        MinorRelationType = minorRelationType;
        IsActive = true;
    }

    public MinorRelationType? MinorRelationType { get; private set; }

    public long? MinorClientId { get; private set; }
    public Client MinorClient { get; private set; }

    public void AddMinorRelation(Client minorRelation)
    {
        _contacts.Add(minorRelation);
    }

    public ErrorOr<Success> RemoveMinorRelation(ClientId contactId)
    {
        var foundMinorRelation = _contacts.FirstOrDefault
            (c => c.Id == contactId);

        if (foundMinorRelation is null)
            return Errors.General.NotFound(nameof(MinorClient));

        if (CanRelationBeDeleted().IsError)
            throw new RemoveMinorRelationException(message:
                Resources.Messages.Validations.MinorClientMustHaveAtLeastOneGuardian);

        _contacts.Remove(foundMinorRelation);

        return Result.Success;
    }

    public ErrorOr<Success> CanRelationBeDeleted()
    {
        if (ClientType == ClientType.Minor && _contacts.Count <= 1)
            return Error.Validation(description:
                Resources.Messages.Validations.MinorClientMustHaveAtLeastOneGuardian);

        return Result.Success;
    }
}

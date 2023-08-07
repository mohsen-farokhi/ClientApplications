using ClientApplications.Domain.Model.Client.ValueObjects;
using ClientApplications.Domain.Model.Client.ValueObjects.Enumerations;
using ErrorOr;
using Framework.Domain;

namespace ClientApplications.Domain.Model.Client;

public partial class Client : AggregateRoot<ClientId>
{
    #region Constant(s)
    public const int MiddleNameMaxLength = 50;
    public const int SuffixMaxLength = 50;
    public const int NicknameMaxLength = 50;
    public const int GenderIdentityMaxLength = 140;
    #endregion /Constant(s)

    internal static ErrorOr<Client> CreateOfAdult
        (FirstName firstName, LastName lastName, 
         BillingType billingType)
    {
        // **************************************************
        var requirementsCheckResult = CheckRequirements
            (firstName, lastName, billingType);

        if (requirementsCheckResult.IsError)
            return requirementsCheckResult.Errors;
        // **************************************************

        return new Client
            (firstName, lastName, ClientType.Adult, billingType);
    }

    private Client
        (FirstName firstName, LastName lastName,
         ClientType clientType, BillingType billingType)
    {
        FirstName = firstName;
        LastName = lastName;
        ClientType = clientType;
        BillingType = billingType;
        IsActive = true;
    }

    private Client()
    {
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public ClientType ClientType { get; private set; }
    public BillingType? BillingType { get; private set; }
    public string? MiddleName { get; private set; }
    public string? Suffix { get; private set; }
    public string? Nickname { get; private set; }
    public bool IsActive { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public Gender? Gender { get; private set; }
    public string? GenderIdentity { get; private set; }

    private readonly List<Client> _contacts = new();
    public IReadOnlyCollection<Client> Contacts => _contacts;

    public ErrorOr<Success> Edit
        (FirstName firstName, LastName lastName, ClientType clientType,
         BillingType billingType, string? middleName, string? suffix,
         string? nickname, bool isActive, DateOnly? birthDate,
         Gender? gender, string? genderIdentity)
    {
        var errors = new List<Error>();

        // **************************************************
        var requirementsCheckResult = CheckRequirements
            (firstName, lastName, billingType);

        if (requirementsCheckResult.IsError)
            errors.AddRange(requirementsCheckResult.Errors);
        // **************************************************

        if (clientType is null)
            errors.Add(Errors.General.Required(nameof(ClientType)));

        middleName = Framework.Core.String.Fix(middleName);

        if (middleName?.Length > MiddleNameMaxLength)
            errors.Add(Errors.General.MaxLength(nameof(MiddleName), MiddleNameMaxLength));

        suffix = Framework.Core.String.Fix(suffix);

        if (suffix?.Length > SuffixMaxLength)
            errors.Add(Errors.General.MaxLength(nameof(Suffix), SuffixMaxLength));

        nickname = Framework.Core.String.Fix(nickname);

        if (nickname?.Length > NicknameMaxLength)
            errors.Add(Errors.General.MaxLength(nameof(Nickname), NicknameMaxLength));

        genderIdentity = Framework.Core.String.Fix(genderIdentity);

        if (genderIdentity?.Length > GenderIdentityMaxLength)
            errors.Add(Errors.General.MaxLength(nameof(GenderIdentity), GenderIdentityMaxLength));

        if (errors.Any())
            return errors;

        FirstName = firstName;
        LastName = lastName;
        ClientType = clientType;
        BillingType = billingType;

        IsActive = isActive;
        BirthDate = birthDate;
        MiddleName = middleName;
        Nickname = nickname;
        Gender = gender;
        GenderIdentity = genderIdentity;

        return Result.Success;
    }

    private static ErrorOr<Success> CheckRequirements
        (FirstName firstName, LastName lastName, BillingType billingType)
    {
        var clientErrors = new List<Error>();

        if (firstName is null)
            clientErrors.Add(Errors.General.Required(nameof(FirstName)));

        if (lastName is null)
            clientErrors.Add(Errors.General.Required(nameof(LastName)));

        if (billingType is null)
            clientErrors.Add(Errors.General.Required(nameof(BillingType)));

        if (clientErrors.Any())
            return clientErrors;

        return Result.Success;
    }
}

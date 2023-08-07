using Framework.Domain;

namespace ClientApplications.Domain.Model.Client.ValueObjects;

public class ClientId : ValueObject
{
    private ClientId()
    {
    }

    public ClientId(long value)
    {
        Value = value;
    }

    public long Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

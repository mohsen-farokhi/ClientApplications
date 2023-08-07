namespace ClientApplications.Domain.Model.Client.Exceptions;

public class RemoveMinorRelationException : Exception
{
    public RemoveMinorRelationException()
    {
    }

    public RemoveMinorRelationException(string message) : base(message)
    {
    }
}

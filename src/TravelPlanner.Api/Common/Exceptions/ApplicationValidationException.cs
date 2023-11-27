namespace TravelPlanner.Api.Common.Exceptions;

public class ApplicationValidationException : Exception
{
    public ApplicationValidationException(string message) : base(message)
    {
    }
}
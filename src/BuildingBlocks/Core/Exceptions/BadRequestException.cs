using System.Net;

namespace FSH.Framework.Core.Exceptions;

/// <summary>
/// Exception representing a 400 Bad Request error.
/// </summary>
public class BadRequestException : CustomException
{
    public BadRequestException()
        : base("Bad request.", Array.Empty<string>(), HttpStatusCode.BadRequest)
    {
    }

    public BadRequestException(string message)
        : base(message, Array.Empty<string>(), HttpStatusCode.BadRequest)
    {
    }

    public BadRequestException(string message, IEnumerable<string> errors)
        : base(message, errors.ToList(), HttpStatusCode.BadRequest)
    {
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException, HttpStatusCode.BadRequest)
    {
    }
}

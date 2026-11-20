namespace BFCAI.Application.Common.Exceptions;

/// <summary>
/// Throw these from services. The API's global exception handler turns them into
/// ProblemDetails responses with the matching HTTP status code.
/// (Validation errors use FluentValidation.ValidationException -> 400 with field errors.)
/// </summary>
public abstract class AppException(string message) : Exception(message);

/// <summary>404</summary>
public class NotFoundException(string message) : AppException(message)
{
    public NotFoundException(string entity, object key) : this($"{entity} '{key}' was not found.") { }
}

/// <summary>400</summary>
public class BadRequestException(string message) : AppException(message);

/// <summary>401</summary>
public class UnauthorizedException(string message = "Unauthorized.") : AppException(message);

/// <summary>403 - authenticated but not allowed (e.g. editing another instructor's course).</summary>
public class ForbiddenException(string message = "You are not allowed to perform this action.") : AppException(message);

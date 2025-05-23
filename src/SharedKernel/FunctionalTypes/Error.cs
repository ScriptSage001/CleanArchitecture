namespace SharedKernel.FunctionalTypes;

/// <summary>
/// Common Record for Error Handling
/// </summary>
/// <param name="Code"></param>
/// <param name="Message"></param>
public record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.",
        ErrorType.Failure);

    public static readonly Error ConditionNotMet =
        new("Error.ConditionNotMet", "The specified condition was not met.", ErrorType.Failure);

    public static readonly Error ValidationError =
        new("Error.ValidationError", "A validation Error occurred.", ErrorType.Validation);

    public static readonly Error NullOrEmptyRequest =
        new("Error.NullOrEmptyRequest", "A validation Error occurred.", ErrorType.Validation);

    public static readonly Error UnauthorizedRequest =
        new("Error.Unauthorized", "An Unauthorized Error occurred.", ErrorType.Unauthorized);

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);
    public static Error Unexpected(string code, string message) => new(code, message, ErrorType.Unexpected);
    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);
    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
    public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);
    public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);
    public static Error Gone(string code, string message) => new(code, message, ErrorType.Gone);
    public static Error NoContent(string code, string message) => new(code, message, ErrorType.NoContent);
    public static Error BadRequest(string code, string message) => new(code, message, ErrorType.BadRequest);
}

/// <summary>
/// Error Types
/// </summary>
public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    Gone,
    NoContent,
    BadRequest,
    Problem,
    None
}
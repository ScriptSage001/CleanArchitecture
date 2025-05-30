namespace SharedKernel.FunctionalTypes;

public interface IValidationResult
{
    public static readonly Error ValidationError = Error.ValidationError;

    Error[] Errors { get; }
}
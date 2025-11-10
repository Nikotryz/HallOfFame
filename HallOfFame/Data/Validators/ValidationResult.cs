namespace HallOfFame.Data.Validators;

public class ValidationResult
{
    public bool IsValid { get; }
    
    public string ErrorMessage { get; }
    
    public ValidationResult(bool isValid, string errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public static ValidationResult Success()
    {
        return new ValidationResult(true);
    }

    public static ValidationResult Failure(string errorMessage)
    {
        return new ValidationResult(false, errorMessage);
    }
}
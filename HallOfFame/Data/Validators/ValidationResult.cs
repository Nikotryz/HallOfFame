namespace HallOfFame.Data.Validators;

/// <summary>
/// Класс для упрощения работы с валидацией. Предоставляет данные о результате валидации.
/// </summary>
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
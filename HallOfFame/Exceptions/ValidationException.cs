namespace HallOfFame.Exceptions;

/// <summary>
/// Исключение, которое вызывается при статусе ответа 400 (Bad Request), то есть при введении пользователем некорректных данных.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException() { }

    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Exception innerException) : base(message, innerException) { }
}

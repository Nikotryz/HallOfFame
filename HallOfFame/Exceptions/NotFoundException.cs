namespace HallOfFame.Exceptions;

/// <summary>
/// Исключение, которое вызывается при статусе ответа 404 (Not Found).
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException() { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

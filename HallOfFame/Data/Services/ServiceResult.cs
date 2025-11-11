namespace HallOfFame.Data.Services;

/// <summary>
/// Класс для упрощения работы с ответами, в которых нужно передать какие-либо данные.
/// </summary>
/// <typeparam name="T">DTO, которое нужно передать в качестве ответа.</typeparam>
public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    
    public T Data { get; set; }

    public string ErrorMessage { get; set; }

    private ServiceResult(bool isSuccess, T data, string errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T>(true, data, null);
    }
}
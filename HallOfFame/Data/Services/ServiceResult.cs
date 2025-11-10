namespace HallOfFame.Data.Services;

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
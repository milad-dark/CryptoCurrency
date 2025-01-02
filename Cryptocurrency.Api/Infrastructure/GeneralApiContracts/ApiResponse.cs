namespace Cryptocurrency.Api.Infrastructure.GeneralApiContracts;

public class ApiResponse
{
    protected ApiResponse(string? errorMessage)
    {
        IsSuccess = string.IsNullOrEmpty(errorMessage);
        Message = errorMessage;
    }

    public bool IsSuccess
    {
        get;
    }
    public string Message
    {
        get;
    }

    public static ApiResponse<T> Success<T>(T data)
    {
        return new ApiResponse<T>(data);
    }

    public static ApiResponse Error(string errorMessage)
    {
        return new ApiResponse(errorMessage);
    }
}

public class ApiResponse<T> : ApiResponse
{
    public ApiResponse(T data) : base(string.Empty)
    {
        Data = data;
    }

    public T Data
    {
        get;
    }
}

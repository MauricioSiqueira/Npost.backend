namespace npost.Middlewares;

public class BusinessException : System.Exception
{
    public string Code { get; }

    public BusinessException(string message, string? code = null)
        : base(message)
    {
        Code = code ?? "business_error";
    }
}
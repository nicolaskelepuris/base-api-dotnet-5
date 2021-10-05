namespace Application.Responses
{
  public class ApiResponse<T>
  {
    public ApiResponse(T data, string error)
    {
      Data = data;
      Error = error;
    }

    public bool Success => string.IsNullOrWhiteSpace(Error) && Data != null;
    public T Data { get; private set; }
    public string Error { get; private set; }
  }
}
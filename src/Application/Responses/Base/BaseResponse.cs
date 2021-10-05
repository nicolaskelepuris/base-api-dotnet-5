using System.Text.Json.Serialization;

namespace Application.Responses.Base
{
  public class BaseResponse
  {
    [JsonIgnore]
    public int? StatusCode { get; set; }
    [JsonIgnore]
    public string ErrorMessage { get; set; }

    public static BaseResponse WithError(int statusCode, string error)
    {
      return new BaseResponse() { StatusCode = statusCode, ErrorMessage = error };
    }
  }
}
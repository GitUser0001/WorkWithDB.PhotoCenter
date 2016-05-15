using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApp.Api.Models.Responces
{
    public class Result<T>
    {
        public static readonly Result<T> Forbidden = new Result<T> { ErrorMessage = "Forbidden", HasError = true };
        public static readonly Result<T> Unauthorized = new Result<T> { ErrorMessage = "Unauthorized", HasError = true };
        public static readonly Result<T> NotExist = new Result<T> { ErrorMessage = "There is no item with such value", HasError = true };
        public static readonly Result<T> BadValue = new Result<T> { ErrorMessage = "You entered bad value", HasError = true };


        [JsonProperty]
        public bool HasError { get; set; }
        [JsonProperty]
        public string ErrorMessage { get; set; }
        [JsonProperty]
        public T Value { get; set; }


        public Result()
        {
        }

        public Result(T value)
        {
            Value = value;
        }

        public Result GetError()
        {                
            return new Result() { ErrorMessage = ErrorMessage };         
        }

        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(value);
        }
    }

    public class Result
    {
        public string ErrorMessage { get; set; }
        public string InfoMessage { get; set; }
    }
}
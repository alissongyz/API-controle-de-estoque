using estoque_tek.Domains.Extensions;
using estoque_tek.Domains.Types;
using System.Net;

namespace estoque_tek.Domains.Models
{
    public class Result
    {
        public Result()
        {
        }

        public Result(bool success, HttpStatusCode statusCode, int errorCode, string message)
        {
            this.Success = success;
            this.StatusCode = statusCode;
            this.Message = message;
            this.ErrorCode = errorCode;
        }

        public Result(bool success, HttpStatusCode statusCode, ErrorCodeType errorCode)
        {
            this.Success = success;
            this.StatusCode = statusCode;
            this.Message = errorCode.GetDescription();
            this.ErrorCode = (int)errorCode;
        }

        public bool Success { get; set; }

        public int ErrorCode { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public static Result BuildBadRequestResult(ErrorCodeType errorCode, string message = null)
        {
            return new Result(false, HttpStatusCode.BadRequest, (int)errorCode, message ?? errorCode.GetDescription());
        }

        public static Result BuildNotFoundResult(ErrorCodeType errorCode, string message = null)
        {
            return new Result(false, HttpStatusCode.NotFound, (int)errorCode, message ?? errorCode.GetDescription());
        }

        public static Result BuildSuccessResult()
        {
            return new Result(true, HttpStatusCode.OK, default, "OK");
        }
    }
}

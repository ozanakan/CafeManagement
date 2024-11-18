using System;
//using Serilog;

namespace CafeOrderManager.Infrastructure.Models
{
    public class Result
    {
        
        public HttpStatus Status { get; set; }

        public string ExceptionType { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionMessageTechnical { get; set; }

        public bool Logout { get; set; }

        public void Error(Exception exception)
        {
            Status = HttpStatus.Error;
            ExceptionType = exception?.GetType()?.FullName;
            ExceptionMessageTechnical = exception?.Message;
            ExceptionMessage = exception?.Message;
            if (ExceptionType?.Contains("BieksperV2") != true)
                ExceptionMessage = "general.error_occured";
            if (ExceptionType?.Contains("UserUnAuthorized") == true)
                Logout = true;
        }

        public void ValidationError(string errorMsg = "")
        {
            Status = HttpStatus.Error;
            ExceptionMessage = errorMsg ?? "validation.not_valid";
            ExceptionMessageTechnical = errorMsg ?? "validation.not_valid";
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public PaginationDto Pagination { get; set; }

        public Result<T> Success(T data)
        {
            Status = HttpStatus.Success;
            Data = data;
            return this;
        }

        public void Success(T data, PaginationDto paginationInfo)
        {
            Status = HttpStatus.Success;
            Pagination = paginationInfo;
            Data = data;
        }

        public void Error(Exception exception)
        {
            base.Error(exception);
            Data = default(T);
        }
    }

    public enum HttpStatus
    {
        Success = 1,
        Error = 2
    }
}
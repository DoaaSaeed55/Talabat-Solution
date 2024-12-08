﻿namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse:ApiResponse
    {
        public ApiExceptionResponse(int statusCode, string? message = null,string? details=null) 
            : base(statusCode, message)
        {
            Details = details;
        }

        public string? Details { get; set; }
    }
}
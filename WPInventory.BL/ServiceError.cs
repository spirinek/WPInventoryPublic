using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace WPInventory.BL
{
    public class ServiceError
    {
        public ServiceError(ErrorCode errorCode, string description)
        {
            ErrorCode = errorCode;
            Description = description;
        }
        public ErrorCode ErrorCode { get; set; }
        public string Description { get; set; }

        public ActionResult ToActionResult()
        {
            switch (ErrorCode)
            {
                case ErrorCode.NotFound:
                    return new NotFoundObjectResult($"[{nameof(ErrorCode.NotFound)}] {Description}");
                default:
                    return new BadRequestObjectResult($"[{nameof(ErrorCode.BadRequest)}] {Description}");
                
            }
        }
    }

    public enum ErrorCode
    {
        BadRequest = 1,
        NotFound = 2
    }
}

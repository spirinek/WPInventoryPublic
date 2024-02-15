using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace WPInventory.BL
{
    public class MediatorResult<T> : MediatorBaseResult where T : class
    {
        public T Data { get; private set; }

        public MediatorResult(T data)
        {
            Data = data;
        }

        public static MediatorResult<T> Success(T data)
        {
            return new MediatorResult<T>(data)
            {
                IsSuccess = true
            };
        }

        public static MediatorResult<T> Failed(ServiceError error)
        {
            var result = new MediatorResult<T>(null)
            {
                IsSuccess = false
            };
            if (error != null)
            {
                result.Error = error;
            }

            return result;
        }

        public static implicit operator ActionResult<T>(MediatorResult<T> result)
        {
            if (result.IsFailed)
            {
                return result.Error.ToActionResult();
            }

            return result.Data;
        }
    }

    public class MediatorResult : MediatorBaseResult
    {
        public static MediatorResult Success()
        {
            return new MediatorResult()
            {
                IsSuccess = true
            };
        }

        public static MediatorResult Failed(ServiceError error)
        {
            var badResult = new MediatorResult
            {
                IsSuccess = false
            };
            if (error != null)
            {
                badResult.Error = error;
            }
            return badResult;
        }

        public static implicit operator ActionResult(MediatorResult result)
        {
            if (result.IsFailed)
            {
              return  result.Error.ToActionResult();
            }
            return new OkResult();
        }
    }

    public class MediatorBaseResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFailed => !IsSuccess;
        public ServiceError Error { get; set; }
    }
}

﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleDirectory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDirectory.Extension.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;

        public ExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var error = new CustomError()
            {
                Message = context.Exception.Message,
                Detail = context.Exception.StackTrace
            };

            if (_env.IsDevelopment())
            {
                error.Message = "An internal server error has been occured.";
                error.Detail = context.Exception.Message;
            }

            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };
        }
    }
}

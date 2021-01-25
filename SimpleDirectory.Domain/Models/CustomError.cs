using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleDirectory.Domain.Models
{
    public class CustomError
    {
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}

using SimpleDirectory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleDirectory.Domain.Models
{
    public class ContactListDTO
    {
        public string Type { get; set; }
        public string Body { get; set; }
    }
}

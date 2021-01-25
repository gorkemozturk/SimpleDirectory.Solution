using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleDirectory.Domain.Models
{
    public class ContactListDTO
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
    }
}

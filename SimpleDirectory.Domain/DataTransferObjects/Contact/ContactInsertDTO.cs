using SimpleDirectory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleDirectory.Domain.Models
{
    public class ContactInsertDTO
    {
        [Required]
        public ContactEnum.ContactTypes Type { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public Guid PersonId { get; set; }
    }
}

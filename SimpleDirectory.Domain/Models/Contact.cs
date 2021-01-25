using SimpleDirectory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SimpleDirectory.Domain.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public ContactEnum.ContactTypes Type { get; set; }

        [Required]
        public string Body { get; set; }

        // Foreign Key
        public Guid PersonId { get; set; }

        // Relations
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
    }
}

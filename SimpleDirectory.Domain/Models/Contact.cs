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
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Location { get; set; }

        // Foreign Key
        public Guid PersonId { get; set; }

        // Relations
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
    }
}

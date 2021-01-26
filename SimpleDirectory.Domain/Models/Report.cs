using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleDirectory.Domain.Models
{
    public class Report
    {
        public Guid Id { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int People { get; set; }

        [Required]
        public int Phones { get; set; }

        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

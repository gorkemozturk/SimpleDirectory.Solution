using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleDirectory.Domain.Models
{
    public class ReportCreateDTO
    {
        [Required]
        public string Location { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDirectory.Domain.Models
{
    public class PersonListDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyName { get; set; }
    }
}

using SimpleDirectory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDirectory.Extension.Interfaces
{
    public interface IPersonService : IBaseService<Person>
    {
        Task<PersonListDTO[]> GetPersonsAsync();
        Task<PersonDetailDTO> GetPerson(Guid id);
        Task<bool> IsPersonExists(Guid id);
    }
}

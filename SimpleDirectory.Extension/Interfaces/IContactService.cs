using SimpleDirectory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDirectory.Extension.Interfaces
{
    public interface IContactService : IBaseService<Contact>
    {
        ContactInsertDTO CreateContactByPerson(Guid personId, ContactInsertDTO contact);
        Task<Contact> GetContactAsync(int id);
    }
}

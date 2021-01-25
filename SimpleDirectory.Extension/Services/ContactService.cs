using AutoMapper;
using SimpleDirectory.Data;
using SimpleDirectory.Domain.Models;
using SimpleDirectory.Extension.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDirectory.Extension.Services
{
    public class ContactService : BaseService<Contact>, IContactService
    {
        private readonly IMapper _mapper;

        public ContactService(DirectoryDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public Contact CreateContactByPerson(Guid personId, ContactInsertDTO contact)
        {
            if (!IsPersonExists(personId) || personId != contact.PersonId)
            {
                return null;
            }

            var model = _mapper.Map<Contact>(contact);
            _context.Contacts.Add(model);

            return model;
        }

        public async Task<Contact> GetContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return null;
            }

            return contact;
        }

        private bool IsPersonExists(Guid id)
        {
            return _context.Persons.Any(p => p.Id == id);
        }
    }
}

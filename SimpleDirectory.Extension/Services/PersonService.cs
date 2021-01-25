using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SimpleDirectory.Data;
using SimpleDirectory.Domain.Models;
using SimpleDirectory.Extension.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDirectory.Extension.Services
{
    public class PersonService : BaseService<Person>, IPersonService
    {
        private readonly IMapper _mapper;

        public PersonService(DirectoryDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PersonDetailDTO> GetPerson(Guid id)
        {
            var person = await _context.Persons
                .Include(p => p.Contacts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                return null;
            }

            return _mapper.Map<PersonDetailDTO>(person);
        }

        public async Task<PersonListDTO[]> GetPersonsAsync()
        {
            return await _context.Persons
                .ProjectTo<PersonListDTO>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}

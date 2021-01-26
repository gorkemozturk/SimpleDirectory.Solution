using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SimpleDirectory.Domain.Models;
using SimpleDirectory.Extension.Interfaces;

namespace SimpleDirectory.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DirectoriesController : ControllerBase
    {
        private readonly IPersonService _person;
        private readonly IContactService _contact;

        public DirectoriesController(IPersonService person, IContactService contact)
        {
            _person = person;
            _contact = contact;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IEnumerable<PersonListDTO>> GetDirectories()
        {
            return await _person.GetPersonsAsync();
        }

        [HttpGet("Person/{id}", Name = "GetPerson")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PersonDetailDTO>> GetPerson([FromRoute] Guid id)
        {
            var person = await _person.GetPerson(id);

            if (person == null)
            {
                var error = new CustomError
                {
                    Detail = new KeyNotFoundException().Message
                };

                return NotFound(error);
            }

            return person;
        }

        [HttpPost("Person")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Person>> PostPerson([FromBody] Person person)
        {
            _person.CreateResource(person);

            if (await _person.SaveChangesAsync() > 0)
            {
                return CreatedAtRoute(nameof(GetPerson), new { id = person.Id }, person);
            }

            throw new InvalidOperationException("The person was not created.");
        }

        [HttpDelete("Person/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Person>> DeletePerson([FromRoute] Guid id)
        {
            var person = await _person.GetResourceAsync(id);

            if (person == null)
            {
                var error = new CustomError
                {
                    Detail = new KeyNotFoundException().Message
                };

                return NotFound(error);
            }

            _person.DeleteResource(person);

            if (await _person.SaveChangesAsync() == 0)
            {
                throw new InvalidOperationException("The person was not deleted.");
            }

            return NoContent();
        }

        [HttpPost("Person/{id}/Contact")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ContactInsertDTO>> CreateContactByPerson([FromRoute] Guid id, [FromBody] ContactInsertDTO contact)
        {
            var result = _contact.CreateContactByPerson(id, contact);

            if (result == null)
            {
                var error = new CustomError
                {
                    Detail = new KeyNotFoundException().Message
                };

                return NotFound(error);
            }

            if (await _contact.SaveChangesAsync() == 0)
            {
                throw new InvalidOperationException("The contact was not inserted to existing person.");
            }

            return CreatedAtRoute(nameof(GetPerson), new { id = contact.PersonId }, contact);
        }

        [HttpDelete("Contact/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Person>> DeleteContact([FromRoute] int id)
        {
            var contact = await _contact.GetContactAsync(id);

            if (contact == null)
            {
                var error = new CustomError
                {
                    Detail = new KeyNotFoundException().Message
                };

                return NotFound(error);
            }

            _contact.DeleteResource(contact);

            if (await _contact.SaveChangesAsync() == 0)
            {
                throw new InvalidOperationException("The contact was not deleted from existing person.");
            }

            return NoContent();
        }
    }
}
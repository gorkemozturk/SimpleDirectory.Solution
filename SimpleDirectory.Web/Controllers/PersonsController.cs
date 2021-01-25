using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDirectory.Domain.Models;
using SimpleDirectory.Extension.Interfaces;

namespace SimpleDirectory.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _person;

        public PersonsController(IPersonService person)
        {
            _person = person;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IEnumerable<PersonListDTO>> GetPersons()
        {
            return await _person.GetPersonsAsync();
        }

        [HttpGet("{id}", Name = "GetPerson")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PersonDetailDTO>> GetPerson([FromRoute] Guid id)
        {
            var person = await _person.GetPerson(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Person>> PostPerson([FromBody] Person person)
        {
            person.Id = Guid.NewGuid();
            _person.CreateResource(person);

            if (await _person.SaveChangesAsync() > 0)
            {
                return CreatedAtRoute(nameof(GetPerson), new { id = person.Id }, person);
            }

            throw new InvalidOperationException("The person was not created.");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Person>> DeletePerson([FromRoute] Guid id)
        {
            var person = await _person.GetResourceAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            _person.DeleteResource(person);

            if (await _person.SaveChangesAsync() == 0)
            {
                throw new InvalidOperationException("The person was not deleted.");
            }

            return NoContent();
        }
    }
}
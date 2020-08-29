using Business.Managers;
using Business.Resources;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleManager _manager;

        public PeopleController(IPeopleManager manager)
        {
            _manager = manager;
        }

        [HttpGet(Name = "GetAllPeople")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _manager.GetPeopleAsync());
        }

        [HttpGet("{id:Guid}", Name = "GetPerson")]
        public IActionResult Get(Guid id)
        {
            var person = _manager.GePersonById(id);

            // Check if the id exists
            if (person is null)
                return NotFound($"No person was found with the id '{id}'");

            return Ok(person);
        }

        [HttpPost(Name = "InsertPerson")]
        public IActionResult Post([FromBody] PersonResource person)
        {
            return Created("/People", _manager.InserPerson(person));
        }

        [HttpPut("{id:Guid}", Name = "UpdatePerson")]
        public IActionResult Put(Guid id, [FromBody] PersonResource person)
        {
            // Check if the id exists before trying to update
            if (_manager.GePersonById(id) is null)
                return NotFound($"No person was found with the id '{id}'");

            return Ok(_manager.UpdatePerson(id, person));
        }

        [HttpDelete("{id:Guid}", Name = "DeletePerson")]
        public IActionResult Delete(Guid id)
        {
            _manager.DeleteEntity(id);

            return NoContent();
        }
    }
}

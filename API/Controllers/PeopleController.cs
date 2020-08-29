using Business.Managers;
using Business.Resources;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController, Route("[controller]"), Authorize]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleManager _manager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public PeopleController(IPeopleManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Find All people
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet(Name = "GetAllPeople")]
        [Produces("application/json", Type = typeof(IEnumerable<PersonResource>))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _manager.GetPeopleAsync());
        }

        /// <summary>
        /// Find Person by id
        /// </summary>
        /// <param name="id">Id of the person</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Person was not found</response>
        [HttpGet("{id:Guid}", Name = "GetPerson")]
        [Produces("application/json", Type = typeof(PersonResource))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            PersonResource person = _manager.GePersonById(id);

            // Check if the id exists
            if (person is null)
                return NotFound($"No person was found with the id '{id}'");

            return Ok(person);
        }

        /// <summary>
        /// Add new perwson to Db
        /// </summary>
        /// <param name="person">Person data to add</param>
        /// <response code="201">Person was created</response>
        /// <response code="400">Invalid model</response>
        [HttpPost(Name = "InsertPerson")]
        [Produces("application/json", Type = typeof(PersonResource))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] PersonResource person)
        {
            return Created("/People", _manager.InserPerson(person));
        }

        /// <summary>
        /// Update a perwson inside Db
        /// </summary>
        /// <param name="id">the person id</param>
        /// <param name="person">New model data</param>
        /// <response code="200">Person was updated</response>
        /// <response code="400">Invalid model</response>
        [HttpPut("{id:Guid}", Name = "UpdatePerson")]
        [Produces("application/json", Type = typeof(PersonResource))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(Guid id, [FromBody] PersonResource person)
        {
            // Check if the id exists before trying to update
            if (_manager.GePersonById(id) is null)
                return NotFound($"No person was found with the id '{id}'");

            return Ok(_manager.UpdatePerson(id, person));
        }

        /// <summary>
        /// Delete a person from db
        /// </summary>
        /// <param name="id">Person id to delete</param>
        /// <response code="204">Person was deleted</response>
        /// <response code="404">Person was not found</response>
        [HttpDelete("{id:Guid}", Name = "DeletePerson")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(Guid id)
        {
            // Check if the id exists before trying to update
            if (_manager.GePersonById(id) is null)
                return NotFound($"No person was found with the id '{id}'");

            _manager.DeleteEntity(id);

            return NoContent();
        }
    }
}

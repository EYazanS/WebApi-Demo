using Business.Managers;
using Business.Resources;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IPeopleManager _manager;

        public PeopleController(ILogger<PeopleController> logger, IPeopleManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpGet(Name = "GetAllPeople")]
        public Task<IEnumerable<PersonResource>> Get()
        {
            return _manager.GetPeopleAsync();
        }

        [HttpGet("{id}", Name = "GetPerson")]
        public PersonResource Get(Guid id)
        {
            return _manager.GePersonById(id);
        }

        [HttpPost(Name = "InsertPerson")]
        public PersonResource Post([FromBody] PersonResource person)
        {
            return _manager.InserPerson(person);
        }

        [HttpPut("{id}", Name = "UpdatePerson")]
        public PersonResource Put(Guid id, [FromBody] PersonResource person)
        {
            return _manager.UpdatePerson(id, person);
        }

        [HttpDelete("{id}", Name = "DeletePerson")]
        public void Delete(Guid id)
        {
            _manager.DeleteEntity(id);
        }
    }
}

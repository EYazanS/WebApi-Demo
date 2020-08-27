using Business.Resources;

using DAL.Models;
using DAL.Repositories;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business.Managers
{
    public interface IPeopleManager
    {
        Task<IEnumerable<PersonResource>> GetPeopleAsync(Expression<Func<PersonEntity, bool>> expression = null);

        Task<PersonResource> FirstOrDefaultAsync(Expression<Func<PersonEntity, bool>> expression);

        PersonResource GePersonById(Guid personId);

        PersonResource InserPerson(PersonResource person);

        PersonResource UpdatePerson(Guid personId, PersonResource person);

        void DeleteEntity(Guid personId);
    }

    public class PeopleManager : IPeopleManager
    {
        private readonly IBaseRepository<PersonEntity, Guid> _repository;
        private readonly ILogger<PeopleManager> _logger;

        public PeopleManager(IBaseRepository<PersonEntity, Guid> repository, ILogger<PeopleManager> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<PersonResource>> GetPeopleAsync(Expression<Func<PersonEntity, bool>> expression = null) => (await _repository
            .GetEntitiesAsync(expression))
            .Select(MapToResource)
            .ToList();

        public async Task<PersonResource> FirstOrDefaultAsync(Expression<Func<PersonEntity, bool>> expression) => MapToResource(await _repository.FirstOrDefaultAsync(expression));
        public PersonResource GePersonById(Guid personId) => MapToResource(_repository.GetEntityById(personId));

        public PersonResource InserPerson(PersonResource person)
        {
            if (!_repository.HasTransaction())
                _repository.StartTransaction();

            try
            {
                var personEntity = new PersonEntity
                {
                    DateOfBirth = person.DateOfBirth,
                    FullName = person.FullName
                };

                _repository.InsertEntity(personEntity);

                _repository.SaveChanges();

                return MapToResource(personEntity);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (_repository.HasTransaction())
                    _repository.EndTransaction();
            }
        }

        public PersonResource UpdatePerson(Guid personId, PersonResource person)
        {
            if (!_repository.HasTransaction())
                _repository.StartTransaction();

            try
            {
                var personEntity = _repository.GetEntityById(personId);

                personEntity.DateOfBirth = person.DateOfBirth;
                personEntity.FullName = person.FullName;

                _repository.UpdateEntity(personEntity);

                _repository.SaveChanges();

                return MapToResource(personEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            finally
            {
                if (_repository.HasTransaction())
                    _repository.EndTransaction();
            }
        }

        public void DeleteEntity(Guid personId)
        {

            if (!_repository.HasTransaction())
                _repository.StartTransaction();

            try
            {
                _repository.DeleteEntity(personId);

                _repository.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            finally
            {
                if (_repository.HasTransaction())
                    _repository.EndTransaction();
            };
        }

        private PersonResource MapToResource(PersonEntity entity)
        {
            return new PersonResource
            {
                Id = entity.Id,
                FullName = entity.FullName,
                DateOfBirth = entity.DateOfBirth
            };
        }
    }
}

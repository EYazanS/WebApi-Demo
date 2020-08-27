using Business.Resources;

using DAL.Models;
using DAL.Repositories;

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

        PersonResource GePersonEntityById(Guid personId);

        PersonResource InserPersonEntity(PersonResource person);

        PersonResource UpdateEntity(Guid personId, PersonResource person);

        void DeleteEntity(Guid personId);
    }

    public class PeopleManager : IPeopleManager
    {
        private readonly IBaseRepository<PersonEntity, Guid> _repository;

        public PeopleManager(IBaseRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PersonResource>> GetPeopleAsync(Expression<Func<PersonEntity, bool>> expression = null) => (await _repository
            .GetEntitiesAsync(expression))
            .Select(MapToResource)
            .ToList();

        public async Task<PersonResource> FirstOrDefaultAsync(Expression<Func<PersonEntity, bool>> expression) => MapToResource(await _repository.FirstOrDefaultAsync(expression));
        public PersonResource GePersonEntityById(Guid personId) => MapToResource(_repository.GetEntityById(personId));

        public PersonResource InserPersonEntity(PersonResource person)
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

        public PersonResource UpdateEntity(Guid personId, PersonResource person)
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

        public void DeleteEntity(Guid personId)
        {

            if (!_repository.HasTransaction())
                _repository.StartTransaction();

            try
            {
                _repository.DeleteEntity(personId);

                _repository.SaveChanges();
            }
            catch (Exception)
            {
                throw;
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

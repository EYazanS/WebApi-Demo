using DAL.Models;

namespace DAL.Repositories
{
    public interface IBaseRepository<TEntity, TEntityId> where TEntity : BaseEntity<TEntityId>
    {

    }    
    public class BaseRepository<TEntity, TEntityId> : IBaseRepository<TEntity, TEntityId> where TEntity : BaseEntity<TEntityId>
    {

    }
}

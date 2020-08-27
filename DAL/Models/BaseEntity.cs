using System;

namespace DAL.Models
{
    public class BaseEntity : BaseEntity<Guid>
    {
    }

    public class BaseEntity<TEntityId>
    {
        public TEntityId Id { get; set; }
    }
}

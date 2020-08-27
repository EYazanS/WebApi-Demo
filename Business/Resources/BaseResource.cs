using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Resources
{
    public class BaseResource : BaseResource<Guid>
    {
    }

    public class BaseResource<TResourceId>
    {
        public TResourceId Id { get; set; }
    }
}

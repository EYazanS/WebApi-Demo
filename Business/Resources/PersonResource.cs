using System;

namespace Business.Resources
{
    public class PersonResource : BaseResource
    {
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}

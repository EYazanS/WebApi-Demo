using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Resources
{
    public class PersonResource : BaseResource
    {
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name length should be between {1} and {0}"), Required]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Person : BaseEntity
    {
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name length should be between {1} and {0}"), Required]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}

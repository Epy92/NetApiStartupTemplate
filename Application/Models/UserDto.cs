using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    [AutoMap(typeof(AspNetUser))]
    public class UserDto
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Ignore]
        public List<string> Roles { get; set; }

        [Ignore]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{6,}$", ErrorMessage = "The password must contain at least 1 uppercase, 1 lowercase, 1 number and 1 special character out of @, #, !, &amp;, %, $, ?, +.")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Ignore]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

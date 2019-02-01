using System;
using System.ComponentModel.DataAnnotations;

namespace Friendster.Controllers.Resources
{
    public class RegisterUserResource
    {
        private string _username;

        [Required]
        public string Username
        {
            get { return _username; }
            set { _username = value?.ToLower(); }
        }

        [Required, StringLength(20, MinimumLength = 1, ErrorMessage = "Password must be 1-20 characters")]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
    }
}

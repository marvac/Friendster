using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
    }
}

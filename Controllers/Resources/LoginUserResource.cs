using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Friendster.Controllers.Resources
{
    public class LoginUserResource
    {
        private string _username;

        [Required]
        public string Username
        {
            get { return _username; }
            set { _username = value?.ToLower(); }
        }

        public string Password { get; set; }
    }
}

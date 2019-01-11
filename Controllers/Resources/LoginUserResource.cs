using System.ComponentModel.DataAnnotations;

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

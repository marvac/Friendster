using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendster.Controllers.Resources
{
    public class UserResource
    {
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value?.ToLower(); }
        }

        public string Password { get; set; }
    }
}

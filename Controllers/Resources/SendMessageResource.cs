using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendster.Controllers.Resources
{
    public class SendMessageResource
    {
        public string Content { get; set; }
        public DateTime MessageSent { get; set; }
    }
}

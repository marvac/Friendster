using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendster.Controllers.Resources
{
    public class MessageResource
    {
        public int Id { get; set; }

        public int SenderId { get; set; }
        public string SenderKnownAs { get; set; }
        public int RecipientId { get; set; }
        public string RecipientKnownAs { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string RecipientPhotoUrl { get; set; }

        public string Content { get; set; }
        public bool MarkedAsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime MessageSent { get; set; }
    }
}

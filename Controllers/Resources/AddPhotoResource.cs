using Microsoft.AspNetCore.Http;
using System;

namespace Friendster.Controllers.Resources
{
    public class AddPhotoResource
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string PublicId { get; set; }
    }
}

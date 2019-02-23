using System;
using System.Collections.Generic;

namespace Friendster.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string KnownAs { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        /// <summary>
        /// Users this user has liked
        /// </summary>
        public ICollection<Like> Likers { get; set; }
        /// <summary>
        /// Users that have liked this user
        /// </summary>
        public ICollection<Like> Likees { get; set; }

        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }

    }
}

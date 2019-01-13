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
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string KnownAs { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public Gender LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Both
    }
}

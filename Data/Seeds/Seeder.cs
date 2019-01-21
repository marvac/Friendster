using Friendster.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Friendster.Data
{
    public class Seeder
    {
        private DataContext _context;

        public Seeder(DataContext context)
        {
            _context = context;
        }

        public void SeedUsers()
        {
            if (!_context.Users.Any())
            {
                Console.WriteLine("Seeding users");
                var contents = File.ReadAllText("Data/Seeds/UserSeeds.json");
                var users = JsonConvert.DeserializeObject<List<User>>(contents);
                foreach (var user in users)
                {
                    CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();

                    _context.Users.Add(user);
                }

                _context.SaveChanges();
            }
        }

        private void CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
            }
        }
    }
}

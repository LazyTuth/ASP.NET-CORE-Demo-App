using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.API.Data;
using Demo.API.Interfaces;
using Demo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DemoContext _context;

        public AuthRepository(DemoContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null){
                return null;
            }
            
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)){
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExist(string userName)
        {
            if (await _context.User.AnyAsync(x => x.UserName == userName))
                return true;
            
            return false;
        }
    }
}
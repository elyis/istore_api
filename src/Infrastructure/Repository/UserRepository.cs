using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using webApiTemplate.src.App.Provider;

namespace istore_api.src.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AddAsync(SignUpBody body, string role)
        {
            var oldUser = await GetAsync(body.Email);
            if (oldUser != null)
                return null;

            var newUser = new User
            {
                Email = body.Email,
                PasswordHash = Hmac512Provider.Compute(body.Password),
                RoleName = role,
            };

            var result = await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<User?> GetAsync(Guid id)
            => await _context.Users
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<User?> GetAsync(string email)
            => await _context.Users
                .FirstOrDefaultAsync(e => e.Email == email);

        public async Task<User?> GetByTokenAsync(string refreshTokenHash)
            => await _context.Users
            .FirstOrDefaultAsync(e => e.Token == refreshTokenHash);


        public async Task<User?> UpdateProfileIconAsync(Guid userId, string filename)
        {
            var user = await GetAsync(userId);
            if (user == null)
                return null;

            user.Image = filename;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<string?> UpdateTokenAsync(string refreshToken, Guid userId, TimeSpan? duration = null)
        {
            var user = await GetAsync(userId);
            if (user == null)
                return null;

            if (duration == null)
                duration = TimeSpan.FromDays(15);

            if (user.TokenValidBefore <= DateTime.UtcNow || user.TokenValidBefore == null)
            {
                user.TokenValidBefore = DateTime.UtcNow.Add((TimeSpan)duration);
                user.Token = refreshToken;
                await _context.SaveChangesAsync();
            }

            return user.Token;
        }
    }
}
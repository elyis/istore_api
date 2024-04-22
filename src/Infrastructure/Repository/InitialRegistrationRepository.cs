using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Infrastructure.Repository
{
    public class InitialRegistrationRepository : IInitialRegistrationRepository
    {
        private readonly AppDbContext _context;

        public InitialRegistrationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InitialRegistration?> Create(string email)
        {
            var initialRegistration = await Get(email);
            if (initialRegistration != null)
                return null;

            initialRegistration = new InitialRegistration
            {
                Email = email
            };
            await _context.InitialRegistrations.AddAsync(initialRegistration);
            await _context.SaveChangesAsync();
            return initialRegistration;

        }

        public async Task<InitialRegistration?> Get(string email)
        {
            return await _context.InitialRegistrations.FirstOrDefaultAsync(e => e.Email == email);
        }

    }
}

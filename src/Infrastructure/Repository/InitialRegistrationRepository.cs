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

        public async Task<InitialRegistration?> Create(string phone)
        {
            var initialRegistration = await GetByPhone(phone);
            if (initialRegistration != null)
                return null;

            initialRegistration = new InitialRegistration
            {
                Phone = phone
            };
            await _context.InitialRegistrations.AddAsync(initialRegistration);
            await _context.SaveChangesAsync();
            return initialRegistration;

        }

        public async Task<InitialRegistration?> GetByPhone(string phone)
        {
            return await _context.InitialRegistrations.FirstOrDefaultAsync(e => e.Phone == phone);
        }

    }
}

using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Shared.Utility;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Infrastructure.Repository
{
    public class PromoCodeRepository : IPromoCodeRepository
    {
        private readonly AppDbContext _context;
        private readonly int _promocodeLength = 8;

        public PromoCodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PromoCode?> AddAsync(CreatePromoCodeBody promoCodeBody)
        {
            PromoCode? promoCode;

            do
            {
                string newCode = CodeGenerator.Generate(_promocodeLength);
                promoCode = await GetOrRemoveExpiredAsync(newCode);

                var now = DateTime.UtcNow;
                if (promoCode != null && (promoCode.IsActive || promoCode.DateExpiration < now))
                {
                    _context.PromoCodes.Remove(promoCode);
                    await _context.SaveChangesAsync();
                }

            } while (promoCode != null && promoCode.IsActive);

            promoCode = new PromoCode
            {
                Code = CodeGenerator.Generate(_promocodeLength),
                Type = promoCodeBody.Type.ToString(),
                Value = promoCodeBody.Value,
                DateExpiration = DateTime.UtcNow.AddDays(promoCodeBody.CountDays)
            };

            promoCode = (await _context.PromoCodes.AddAsync(promoCode))?.Entity;
            await _context.SaveChangesAsync();

            return promoCode;
        }

        public async Task<bool> RemoveAsync(string code)
        {
            var promocode = await GetOrRemoveExpiredAsync(code);
            if (promocode == null)
                return true;

            _context.PromoCodes.Remove(promocode);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<PromoCode>> GetAllAsync(bool isActive)
        {
            var now = DateTime.UtcNow;
            return await _context.PromoCodes
                .Where(e => e.IsActive == isActive && e.DateExpiration > now)
                .ToListAsync();
        }

        public async Task<bool> ActivePromocode(string code)
        {
            var promocode = await GetOrRemoveExpiredAsync(code);
            if (promocode == null)
                return false;

            promocode.IsActive = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PromoCode?> GetOrRemoveExpiredAsync(string code)
        {
            var codeLowercase = code.ToLower();
            var promoCode = await _context.PromoCodes
                .FirstOrDefaultAsync(e => e.Code.ToLower() == codeLowercase);

            var now = DateTime.UtcNow;

            if (promoCode != null && (promoCode.DateExpiration < now || promoCode.IsActive))
            {
                _context.PromoCodes.Remove(promoCode);
                await _context.SaveChangesAsync();
                promoCode = null;
            }

            return promoCode;
        }
    }
}
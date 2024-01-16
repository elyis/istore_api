using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Infrastructure.Repository
{
    public class ProductCharacteristicRepository : IProductCharacteristicRepository
    {
        private readonly AppDbContext _context;

        public ProductCharacteristicRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductCharacteristicVariant?> AddAsync(CreateProductCharacteristicBody characteristicBody, Product product)
        {
            var characteristic = await _context.ProductCharacteristics
                .FirstOrDefaultAsync(e => e.Name == characteristicBody.Name);

            if(characteristic == null)
            {
                characteristic = new ProductCharacteristic
                {
                    Name = characteristicBody.Name,
                };
                await _context.ProductCharacteristics.AddAsync(characteristic);
                await _context.SaveChangesAsync();
            }

            var characteristicVariant = await _context.ProductCharacteristicVariants
                .FirstOrDefaultAsync(e => 
                    e.CharacteristicName == characteristic.Name &&
                    e.ProductId == product.Id);

            if(characteristicVariant != null)
                return null;
            
            var values = characteristicBody.Variants.Select(e => e.Value);
            var growthToValues = characteristicBody.Variants.Select(e => e.IncreaseInPrice);
            characteristicVariant = new ProductCharacteristicVariant
            {
                Characteristic = characteristic,
                Product = product,
                Values = string.Join(";", values),
                GrowthToValues = string.Join(";", growthToValues),
            };

            characteristicVariant = (await _context.AddAsync(characteristicVariant))?.Entity;
            await _context.SaveChangesAsync();
            return characteristicVariant;
        }

        public async Task<IEnumerable<ProductCharacteristic>> GetAllAsync()
        {
            var result = await _context.ProductCharacteristics.ToListAsync();
            return result;
        }
    }
}
using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Product?> AddAsync(DeviceModel deviceModel, CreateProductBody productBody)
        {
            var product = new Product
            {
                Name = productBody.Name,
                Description = productBody.Description,
                DeviceModel = deviceModel
            };

            product = (await _context.Products.AddAsync(product))?.Entity;
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var product = await GetAsync(id);
            if(product == null)
                return true;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<Product>> GetAllByPatternName(string pattern)
        {
            pattern = pattern.Replace(" ", string.Empty).Replace("\t", string.Empty);
            pattern = pattern.ToLower();

            return await _context.Products
                .Where(e => 
                    EF.Functions.Like(e.Name.ToLower(), $"%{pattern}%"))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAll(string deviceModelName)
            => await _context.Products
                .Include(e => e.ProductCharacteristics)
                .Include(e => e.ProductConfigurations)
                    .ThenInclude(e => e.Characteristics)
                        .ThenInclude(e => e.ProductCharacteristic)
                .Where(e => e.DeviceModelName == deviceModelName)
                .ToListAsync();

        public async Task<Product?> GetAsync(Guid id)
            => await _context.Products.FindAsync(id);

        public async Task<Product?> UpdateAsync(UpdatingProductBody productBody)
        {
            var product = await GetAsync(productBody.Id);
            if(product == null)
                return null;

            product.Name = productBody.Name;
            product.Description = productBody.Description;
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<IEnumerable<ProductConfiguration>> GetAll(IEnumerable<Guid> configIds)
            => await _context.ProductConfigurations
                .Where(e => 
                    configIds.Contains(e.Id))
                .ToListAsync();
    }
}
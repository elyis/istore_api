using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Infrastructure.Repository
{
    public class DeviceModelRepository : IDeviceModelRepository
    {
        private readonly AppDbContext _context;

        public DeviceModelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeviceModel?> AddAsync(CreateDeviceModelBody deviceModelBody, ProductCategory productCategory)
        {
            var deviceModel = await GetAsync(deviceModelBody.Name);
            if(deviceModel != null)
                return null;

            deviceModel = new DeviceModel
            {
                Name = deviceModelBody.Name,
                ProductCategory = productCategory
            };


            deviceModel = (await _context.DeviceModels.AddAsync(deviceModel))?.Entity;
            await _context.SaveChangesAsync();
            return deviceModel;
        }

        public async Task<IEnumerable<DeviceModel>> GetAllAsync(string productCategoryName)
            => await _context.DeviceModels
                .Where(e => e.ProductCategoryName.ToLower() == productCategoryName.ToLower())
                .ToListAsync();

        public async Task<DeviceModel?> GetAsync(string name)
            => await _context.DeviceModels
            .FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower());
    }
}
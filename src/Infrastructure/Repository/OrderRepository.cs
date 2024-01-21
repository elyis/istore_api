using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> AddAsync(CreateOrderBody orderBody, List<ProductConfiguration> configurations, float totalSum)
        {
            var products = new List<OrderProduct>();
            foreach(var config in configurations)
            {
                var product = new OrderProduct
                {
                    ProductConfiguration = config,
                    Count = orderBody.Configurations.First(e => e.ConfigurationId == config.Id).Count,
                };
                products.Add(product);
            }

            var order = new Order
            {
                Fullname = orderBody.Fullname,
                Comment = orderBody.Comment,
                CommunicationMethod = orderBody.CommunicationMethod.ToString(),
                Email = orderBody.Email,
                Phone = orderBody.Phone,
                TotalSum = totalSum,
                Products = products,
            };

            order = (await _context.Orders.AddAsync(order))?.Entity;
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order?> GetAsync(Guid id)
            => await _context.Orders
                .Include(e => e.Products)
                    .ThenInclude(e => e.ProductConfiguration)
                        .ThenInclude(e => e.Product)
                .Include(e => e.Products)
                    .ThenInclude(e => e.ProductConfiguration)
                        .ThenInclude(e => e.Characteristics)
                            .ThenInclude(e => e.ProductCharacteristic)
                .FirstOrDefaultAsync(e => e.Id == id);
    }
}
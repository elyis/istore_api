using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Response;
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
            foreach (var config in configurations)
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

        public async Task<AnalyticBody> GetAnalyticBody()
        {
            var date = DateTime.UtcNow.AddMonths(-1);

            var orders = await _context.Orders
                .Include(e => e.Products)
                    .ThenInclude(e => e.ProductConfiguration)
                        .ThenInclude(e => e.Product)
                            .ThenInclude(e => e.ProductCharacteristics)
                .Where(e => e.CreatedAt >= date)
                .ToListAsync();

            var productAnalytics = orders.SelectMany(e => e.Products.Select(e => e.ToProductAnalyticBody())).GroupBy(e => e.Name);
            var temp = productAnalytics.Select(e => new ProductAnalyticBody
            {
                Count = e.Sum(e => e.Count),
                Name = e.Key,
                Images = e.FirstOrDefault()?.Images ?? new List<string>()
            })
            .ToList();

            var analyticBody = new AnalyticBody
            {
                CountOrders = orders.Count,
                ProductAnalytics = temp,
                AverageSum = orders.Any() ? orders.Average(e => e.TotalSum) : 0,
                CountPurchasedGoods = temp.Sum(e => e.Count),
                MaxCostProduct = orders.MaxBy(e => e.TotalSum)?.TotalSum ?? 0,
                MinCostProduct = orders.MinBy(e => e.TotalSum)?.TotalSum ?? 0,
            };

            return analyticBody;
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
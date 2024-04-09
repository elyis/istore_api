using istore_api.src.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace istore_api.src.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IConfiguration config
        )
        : base(options)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCharacteristic> ProductCharacteristics { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<BlogTopic> Blogs { get; set; }

        public DbSet<ProductConfiguration> ProductConfigurations { get; set; }
        public DbSet<ProductConfigCharacteristic> ProductConfigCharacteristics { get; set; }
        public DbSet<InitialRegistration> InitialRegistrations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DeviceModel>()
                .HasOne(e => e.ProductCategory)
                .WithMany(e => e.DeviceModels)
                .HasForeignKey(e => e.ProductCategoryName);

            modelBuilder.Entity<Product>()
                .HasOne(e => e.DeviceModel)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.DeviceModelName);


            modelBuilder.Entity<OrderProduct>().HasKey(e => new
            {
                e.OrderId,
                e.ProductConfigurationId
            });


            modelBuilder.Entity<ProductConfigCharacteristic>().HasKey(e => new
            {
                e.ProductCharacteristicId,
                e.ProductConfigurationId
            });

            base.OnModelCreating(modelBuilder);
        }
    }

}
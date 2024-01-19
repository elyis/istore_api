using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Enums;
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

        public async Task<ProductCharacteristic?> AddAsync(Product product, CreateCharacteristicBody characteristicBody, CharacteristicType characteristicType)
        {
            var productCharacteristic = await GetAsync(characteristicBody.Name, product.Id);
            if(productCharacteristic != null)
                return null;

            productCharacteristic = new ProductCharacteristic
            {
                Name = characteristicBody.Name,
                Product = product,
                Values = string.Join(";", characteristicBody.Values),
                Type = characteristicType.ToString()
            };
            productCharacteristic = (await _context.ProductCharacteristics.AddAsync(productCharacteristic))?.Entity;
            await _context.SaveChangesAsync();

            await CreateConfigurations(product.Id, true);
            return productCharacteristic;
        }

        public async Task AddImagesToProduct(List<ProductCharacteristic> productCharacteristics, Guid productId)
        {
            var colorType = CharacteristicType.Color.ToString();
            var characteristics = await GetAll(productId);
            var colorCharacteristics = characteristics.Where(e => e.Type == colorType && e.Color != null).ToList();
            var isFirstColor = !colorCharacteristics.Any();

            var addedCharacteristics = new List<ProductCharacteristic>();
            foreach(var productCharacteristic in productCharacteristics)
            {
                var colorLowercase = productCharacteristic.Color.ToLower();
                var colorCharacteristic = colorCharacteristics.FirstOrDefault(e => e.Color.ToLower() == colorLowercase);

                if(colorCharacteristic != null)
                {
                    var filenames = colorCharacteristic.Values.Split(";").ToList();
                    var temp = productCharacteristic.Values.Split(";");
                    filenames.AddRange(temp);
                    colorCharacteristic.Values = string.Join(";", filenames);
                }
                else
                {
                    await _context.ProductCharacteristics.AddAsync(productCharacteristic);
                    colorCharacteristics.Add(productCharacteristic);
                    addedCharacteristics.Add(productCharacteristic);
                }
            }

            if(addedCharacteristics.Any()){
                if(isFirstColor)
                {
                    var configurations = await _context.ProductConfigurations.Where(e => e.ProductId == productId).ToListAsync();
                    _context.ProductConfigurations.RemoveRange(configurations);
                    await _context.SaveChangesAsync();
                }

                var addedColorCharacteristic = addedCharacteristics.Where(e => e.Type == colorType);
                if(addedColorCharacteristic.Any())
                    await CreateConfigurations(productId, addedCharacteristics);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ProductCharacteristic?> GetAsync(string name, Guid productId)
        {
            var nameLowercase = name.ToLower();
            return await _context.ProductCharacteristics
                .FirstOrDefaultAsync(e => 
                    e.Name.ToLower() == nameLowercase && e.ProductId == productId);
        }

        public async Task<IEnumerable<ProductCharacteristic>> GetAll(Guid productId)
            => await _context.ProductCharacteristics
                .Where(e => e.ProductId == productId)
                .ToListAsync();

        private async Task CreateConfigurations(Guid productId, List<ProductCharacteristic> colorCharacteristics)
        {
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == productId);
            if(product == null)
                return;

            var characteristics = await _context.ProductCharacteristics
                .Where(e => e.ProductId == productId)
                .ToListAsync();
            
            var textCharacteristics = await _context.ProductCharacteristics
                .Where(e => 
                    e.ProductId == productId &&
                    e.Type == CharacteristicType.Text.ToString())
                .ToListAsync();

            if(!colorCharacteristics.Any())
            {
                await CreateProductConfigurations(textCharacteristics, product);
                return;
            }

            foreach(var colorCharacteristic in colorCharacteristics)
            {
                var temp = textCharacteristics.Append(colorCharacteristic).ToList();
                await CreateProductConfigurations(temp, product);
            }
        }

        private async Task CreateProductConfigurations(List<ProductCharacteristic> characteristics, Product product)
        {
            var combinations = GenerateCombinations(characteristics, 0, new List<ProductCharacteristic>());
            var productConfigurations = new List<ProductConfiguration>();

            foreach(var combination in combinations)
            {
                var productConfigCharacteristics = new List<ProductConfigCharacteristic>();
                foreach(var productCharacteristic in combination)
                {
                    var newProductConfigCharacteristic = new ProductConfigCharacteristic
                    {
                        Value = productCharacteristic.Values,
                        ProductCharacteristic = characteristics.First(e => e.Id == productCharacteristic.Id)
                    };
                    productConfigCharacteristics.Add(newProductConfigCharacteristic);
                }
                var productConfiguration = new ProductConfiguration
                {
                    Price = 0,
                    Product = product,
                    Characteristics = productConfigCharacteristics,
                };
                productConfigurations.Add(productConfiguration);
            }

            await _context.ProductConfigurations.AddRangeAsync(productConfigurations);
            await _context.SaveChangesAsync();
        }


        private async Task CreateConfigurations(Guid productId, bool isRemoveOldConfigurations = false)
        {
            if(isRemoveOldConfigurations)
            {
                var configurations = await _context.ProductConfigurations.Where(e => e.ProductId == productId).ToListAsync();
                _context.ProductConfigurations.RemoveRange(configurations);
                await _context.SaveChangesAsync();
            }

            var colorCharacteristics = await _context.ProductCharacteristics
                .Where(e => 
                    e.Type == CharacteristicType.Color.ToString() && 
                    e.ProductId == productId
                ).ToListAsync();
            
            await CreateConfigurations(productId, colorCharacteristics);
        }

        static List<List<ProductCharacteristic>> GenerateCombinations(List<ProductCharacteristic> characteristics, int index, List<ProductCharacteristic> currentCombination)
        {
            var result = new List<List<ProductCharacteristic>>();
            if (index == characteristics.Count)
            {
                result.Add(new List<ProductCharacteristic>(currentCombination));
                return result;
            }

            var currentCharacteristic = characteristics[index];
            var textCharacteristicType = CharacteristicType.Text.ToString();
            var values = new List<string>();
            if(currentCharacteristic.Type == textCharacteristicType)
                values = currentCharacteristic.Values.Split(";").ToList();
            else
                values = new List<string> { currentCharacteristic.Color };
            

            for (int i = 0; i < values.Count; i++)
            {
                var newValue = new ProductCharacteristic
                {
                    Id = currentCharacteristic.Id,
                    Name = currentCharacteristic.Name,
                    Color = currentCharacteristic.Color,
                    Hex = currentCharacteristic.Hex,
                    Type = currentCharacteristic.Type,
                    Values = values[i],
                };

                currentCombination.Add(newValue);
                result.AddRange(GenerateCombinations(characteristics, index + 1, currentCombination));
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }

            return result;
        }

        public async Task<ProductConfiguration?> UpdateProductConfiguration(UpdateProductConfigurationBody updateProduct)
        {
            var configuration = await _context.ProductConfigurations
                .FirstOrDefaultAsync(e => e.Id == updateProduct.ConfigurationId);

            if(configuration == null)
                return null;

            configuration.Price = updateProduct.Price;
            await _context.SaveChangesAsync();
            return configuration;
        }
    }
}
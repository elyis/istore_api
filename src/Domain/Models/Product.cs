using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public string DeviceModelName { get; set; }
        public DeviceModel DeviceModel { get; set; }
        public List<OrderProducts> Orders { get; set; } = new();
        public List<ProductImage> ProductImages { get; set; } = new();
        public List<ProductCharacteristicVariant> CharacteristicVariants { get; set; } = new();

        private List<Filter> ToFilters()
        {
            var filters = new List<Filter>();
            var colorFilter = ProductImages
                .Where(e => e.IsPreviewImage)
                .Select(e => e.ToElem())
                .ToList();

            if (colorFilter.Any())
            {
                filters.Add(new Filter
                {
                    Name = "Color",
                    Type = FilterType.Color,
                    Elems = colorFilter
                });
            }

            filters.AddRange(CharacteristicVariants.Select(e => e.ToFilter()));
            return filters;
        }

        public ProductBody ToProductBody()
        {
            return new ProductBody
            {
                ProductId = Id,
                Name = Name,
                Filters = ToFilters(),
                ProductVariantBodies = ToProductVariantBodies()
            };
        }

        public List<ProductVariantBody> ToProductVariantBodies()
        {
            var result = new List<ProductVariantBody>();
            var productImagesGrouped = ProductImages.Where(e => e.IsPreviewImage).GroupBy(e => e.Color);

            var characteristicsPairs = CharacteristicVariants.Select(e => e.ToCharacteristicPairs()).ToList();
            var uniquePermutations = GenerateUniquePermutations(characteristicsPairs);
            
            if(!ProductImages.Any())
            {
                foreach(var permutation in uniquePermutations)
                {
                    var priceModifier = permutation.Sum(e => e.PriceModifier);
                    var produDb = new ProductVariantBody
                    {
                        Color = null,
                        Characteristics = permutation.ToList(),
                        TotalPrice = Price + priceModifier
                    };
                    result.Add(produDb);
                }
            }

            foreach(var productImageGroup in productImagesGrouped)
            {
                foreach(var permutation in uniquePermutations)
                {
                    var priceModifier = permutation.Sum(e => e.PriceModifier);
                    var produDb = new ProductVariantBody
                    {
                        Color = productImageGroup.Key,
                        Characteristics = permutation.ToList(),
                        ImageUrls = productImageGroup.Select(e => $"{Constants.webPathToProductIcons}{e.Filename}").ToList(),
                        TotalPrice = Price + priceModifier
                    };
                    result.Add(produDb);
                }
            }

            return result;
        }

        static IEnumerable<IEnumerable<T>> GenerateUniquePermutations<T>(List<List<T>> nestedArrays)
        {
            var result = new List<IEnumerable<T>>();
            GeneratePermutations(nestedArrays, 0, new List<T>(), result);
            return result;
        }

        static void GeneratePermutations<T>(List<List<T>> nestedArrays, int currentIndex, List<T> currentPermutation, List<IEnumerable<T>> result)
        {
            if (currentIndex == nestedArrays.Count)
            {
                result.Add(currentPermutation.ToList());
                return;
            }

            foreach (var item in nestedArrays[currentIndex].Distinct())
            {
                currentPermutation.Add(item);
                GeneratePermutations(nestedArrays, currentIndex + 1, currentPermutation, result);
                currentPermutation.RemoveAt(currentPermutation.Count - 1);
            }
        }
    }
}
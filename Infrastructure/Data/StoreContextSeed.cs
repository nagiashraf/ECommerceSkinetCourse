using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext context)
        {
            if (!context.ProductTypes.Any())
            {
                List<ProductType>? productTypes = GetDeserializedbjectsAsync<ProductType>("types.json");
                if (productTypes is not null)
                {
                    context.ProductTypes.AddRange(productTypes);
                }
            }

            if (!context.ProductBrands.Any())
            {
                List<ProductBrand>? productBrands = GetDeserializedbjectsAsync<ProductBrand>("brands.json");
                if (productBrands is not null)
                {
                    context.ProductBrands.AddRange(productBrands);
                }
            }

            if (!context.Products.Any())
            {
                List<Product>? products = GetDeserializedbjectsAsync<Product>("products.json");
                if (products is not null)
                {
                    context.Products.AddRange(products);
                }
            }
            
            await context.SaveChangesAsync();
        }

        private static List<T>? GetDeserializedbjectsAsync<T>(string fileName)
        {
            string directoryPath = GetJsonFileDirectoryPath();
            string filePath = Path.Combine(directoryPath, fileName);
            string jsonObjects = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>?>(jsonObjects);
        }

        private static string GetJsonFileDirectoryPath()
        {
            string directoryPath;
            string currentDirectory = Environment.CurrentDirectory;
            if (currentDirectory.Contains(".Tests"))
            {
                directoryPath = "../../../../Infrastructure/Data/SeedData";
            }
            else
            {
                directoryPath = "../Infrastructure/Data/SeedData";
            }
            return directoryPath;
        }
    }
}
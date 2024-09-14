using System.Text.Json;      // Importing the namespace for JSON serialization and deserialization
using Core.Entities;

namespace Infrastructure.Data;
public class StoreContextSeed
{
    // This method seeds the database with initial data if it's empty
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.Products.Any())                 // Checking if the Products table is empty
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json"); // Reading product data from a JSON file

            var products = JsonSerializer.Deserialize<List<Product>>(productsData); // Deserializing the JSON data into a list of Product objects

            if (products == null) return;            // If deserialization fails, exit the method

            context.Products.AddRange(products);     // Adding the deserialized product list to the context

            await context.SaveChangesAsync();        // Saving the changes to the database
        }
    }
}

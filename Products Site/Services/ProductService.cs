using ASP_NET_03._Razor_Pages_Product_Site.Data;
using ASP_NET_03._Razor_Pages_Product_Site.Models;
using Bogus;

namespace ASP_NET_03._Razor_Pages_Product_Site.Services;

public class ProductService(IProductRepository productRepository) {
    public Task<IEnumerable<Product>> GetProductsAsync()
        => productRepository.GetProductsAsync();

    public Task<Product> GetProductById(int id)
        => productRepository.GetProductByIdAsync(id);

    public Product AddProduct(Product product)
    {
        var faker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Int(1));
        product.Id = faker.Generate().Id;
        if (product.Count > 0) product.IsAvailable = true;
        productRepository.AddProduct(product);
        return product;
    }
}

namespace ShoppingCart.BFF.Product.Core;

public interface IProductRepository
{
    Task<List<ProductEntity>> GetAllAsync();
    Task<ProductEntity> GetByIdAsync(Guid id);
    Task<ProductEntity> AddProductAsync(string name, string description, Double price);
    Task<ProductEntity> AddReviewForProductAsync(Guid id, Review review);
}

public class InMemoryProductRepository: IProductRepository
{
    private IEnumerable<ProductEntity> _products;

    public InMemoryProductRepository(IEnumerable<ProductEntity>  products)
    {
        _products = products;
    }
    
    public Task<List<ProductEntity>> GetAllAsync()
    {
        return Task.Run(() => _products.ToList());
    }

    public Task<ProductEntity> GetByIdAsync(Guid id)
    {
        return Task.Run(() => _products.First(p => p.Id.Equals(id)));
    }

    public Task<ProductEntity> AddProductAsync(string name, string description, Double price)
    {
        var product = new ProductEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            Reviews = Enumerable.Empty<Review>(),
        };

        _products = _products.Concat(new []{ product });

        return Task.Run(() => product);
    }

    public Task<ProductEntity> AddReviewForProductAsync(Guid id, Review review)
    {
        var product = _products.First(p => p.Id.Equals(id));
        product.Reviews = product.Reviews.Concat(new [] { review });

        return Task.Run(() => product);
    }
}
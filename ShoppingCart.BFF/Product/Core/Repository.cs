namespace ShoppingCart.BFF.Product.Core;

public interface IProductRepository
{
    Task<List<ProductEntity>> GetAllAsync();
    Task<ProductEntity> GetByIdAsync(Guid id);
    Task<ProductEntity> AddProductAsync(ProductInput productInput);
    Task<ProductEntity> AddReviewForProductAsync(Guid id, ReviewEntity reviewEntity);
    Task<ProductEntity> DeleteProductById(Guid id);
}

public class InMemoryProductRepository : IProductRepository
{
    private IEnumerable<ProductEntity> _products;

    public InMemoryProductRepository(IEnumerable<ProductEntity> products)
    {
        _products = products;
    }

    public Task<List<ProductEntity>> GetAllAsync()
    {
        return Task.Run(() => _products.ToList());
    }

    public Task<ProductEntity> GetByIdAsync(Guid id)
    {
        return Task.Run(() => FindFirstById(id));
    }

    public Task<ProductEntity> AddProductAsync(ProductInput productInput)
    {
        var product = new ProductEntity
        {
            Id = Guid.NewGuid(),
            Name = productInput.Name,
            Description = productInput.Description,
            Price = productInput.Price,
            Reviews = Enumerable.Empty<ReviewEntity>(),
        };

        _products = _products.Concat(new[] {product});

        return Task.Run(() => product);
    }

    public Task<ProductEntity> AddReviewForProductAsync(Guid id, ReviewEntity reviewEntity)
    {
        var product = FindFirstById(id);
        product.Reviews = product.Reviews.Concat(new[] {reviewEntity});

        return Task.Run(() => product);
    }

    public Task<ProductEntity> DeleteProductById(Guid id)
    {
        var productToDelete = FindFirstById(id);

        var newProducts = Enumerable.Empty<ProductEntity>();
        foreach (var product in _products)
        {
            if (product != productToDelete) newProducts = newProducts.Append(product);
        }

        _products = newProducts;
        
        return Task.Run(() => productToDelete);
    }

    private ProductEntity FindFirstById(Guid id) => _products.First(p => p.Id.Equals(id));
}
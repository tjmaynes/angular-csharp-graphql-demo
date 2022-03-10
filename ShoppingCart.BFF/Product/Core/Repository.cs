namespace ShoppingCart.BFF.Product.Core;

public interface IProductRepository
{
    Task<List<ProductEntity>> GetAllAsync();
    Task<ProductEntity> GetByIdAsync(Guid id);
    Task<ProductEntity> AddProductAsync(string name, string description, Double price);
    Task<ProductEntity> AddReviewForProductAsync(Guid id, Review review);
}

public class InMemoryProductRepository : IProductRepository
{
    private IEnumerable<ProductEntity> _products;

    public InMemoryProductRepository()
    {
        _products = new[]
        {
            new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = "Orange Juice",
                Description = "Florida's best!",
                Price = 5.99,
                Reviews = new[]
                {
                    new Review
                    {
                        Id = Guid.NewGuid(),
                        Reviewer = "Jerry Smith",
                        Content = "The best OJ I've ever had!",
                        Stars = Rating.AMAZING
                    },
                    new Review
                    {
                        Id = Guid.NewGuid(),
                        Reviewer = "Rick Sanchez",
                        Content = "I've had better...",
                        Stars = Rating.GOOD
                    }
                }
            },
            new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = "Graham Crackers",
                Description = "Cinnamon, milk and ginger",
                Price = 2.99,
                Reviews = new[]
                {
                    new Review
                    {
                        Id = Guid.NewGuid(),
                        Reviewer = "Jerry Smith",
                        Content = "I love this brand! So good in dunked in milk.",
                        Stars = Rating.AMAZING
                    },
                    new Review
                    {
                        Id = Guid.NewGuid(),
                        Reviewer = "Morty Smith",
                        Content = "Totally agree, Dad! So good dunked in milk.",
                        Stars = Rating.AMAZING
                    }
                }
            }
        };
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

        _products = _products.Concat(new[] {product});

        return Task.Run(() => product);
    }

    public Task<ProductEntity> AddReviewForProductAsync(Guid id, Review review)
    {
        var product = _products.First(p => p.Id.Equals(id));
        product.Reviews = product.Reviews.Concat(new[] {review});

        return Task.Run(() => product);
    }
}
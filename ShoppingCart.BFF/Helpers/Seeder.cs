using ShoppingCart.BFF.Product.Core;

namespace ShoppingCart.BFF.Helpers;

public interface ISeeder
{
    public IReadOnlyList<ProductEntity> Products { get; }
}

public class Seeder : ISeeder
{
    public IReadOnlyList<ProductEntity> Products { get; }

    public Seeder()
    {
        Products = new[]
        {
            new ProductEntity
            {
                Id = Guid.NewGuid(),
                Name = "Orange Juice",
                Description = "Florida's best!",
                Manufacturer = "Florida Orange",
                Price = 5.99,
                Reviews = new[]
                {
                    new ReviewEntity
                    {
                        Id = Guid.NewGuid(),
                        Reviewer = "Jerry Smith",
                        Content = "The best OJ I've ever had!",
                        Stars = Rating.AMAZING
                    },
                    new ReviewEntity
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
                Manufacturer = "Keeblers",
                Price = 2.99,
                Reviews = new[]
                {
                    new ReviewEntity
                    {
                        Id = Guid.NewGuid(),
                        Reviewer = "Jerry Smith",
                        Content = "I love this brand! So good in dunked in milk.",
                        Stars = Rating.AMAZING
                    },
                    new ReviewEntity
                    {
                        Id = Guid.NewGuid(),
                        Reviewer = "Morty Smith",
                        Content = "Totally agree, Dad! So good dunked in milk.",
                        Stars = Rating.AMAZING
                    }
                }
            }
        }.ToList();
    }
}
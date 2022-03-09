namespace ShoppingCart.BFF.Product.Core;

public class ProductEntity {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    public IEnumerable<Review> Reviews { get; set; }
}

public enum Rating: int
{
    Amazing = 5,
    Good = 4,
    Okay = 3,
    Poor = 2,
    Bad = 1
}

public class Review
{
    public Guid Id { get; set; }
    public string Reviewer { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Rating Stars { get; set; }
}
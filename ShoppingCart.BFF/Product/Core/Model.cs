namespace ShoppingCart.BFF.Product.Core;

public class ProductEntity {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = null!;
    public double Price { get; set; }
    public IEnumerable<ReviewEntity> Reviews { get; set; } = null!;
}

public class ProductInput
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = null!;
    public double Price { get; set; } 
}

public enum Rating
{
    AMAZING,
    GOOD,
    OKAY,
    POOR,
    BAD
}

public class ReviewEntity
{
    public Guid Id { get; set; }
    public string Reviewer { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Rating Stars { get; set; }
}
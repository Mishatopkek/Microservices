namespace Discount.API.SqlQueries;

public static class DiscountRepositoryQueries
{
    public const string Get = "SELECT * FROM Coupon WHERE ProductName = @ProductName";
    public const string Create = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)";
    public const string Update = "UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id";
    public const string Delete = "DELETE FROM Coupon WHERE ProductName = @ProductName";
}
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetPreconfiguredOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
        }
    }

    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
        {
            new Order() {
                UserName = "swn", 
                TotalPrice = 350,
                FirstName = "Mehmet", 
                LastName = "Ozkaya", 
                EmailAddress = "ezozkme@gmail.com", 
                AddressLine = "Bahcelievler", 
                Country = "Ukraine",
                State = "Kharkiv",
                ZipCode = "61000",
                CardName = "Mater Visa",
                CardNumber = "424242424242424242",
                Expiration = "19/91",
                CVV = "123",
                PaymentMethod = 12
            }
        };
    }
}
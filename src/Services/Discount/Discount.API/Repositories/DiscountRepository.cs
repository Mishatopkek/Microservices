using Dapper;
using Discount.API.Entities;
using Discount.API.SqlQueries;
using Npgsql;

namespace Discount.API.Repositories;

public class DiscountRepository : BaseRepository, IDiscountRepository
{
    public DiscountRepository(IConfiguration configuration) 
        : base(configuration.GetValue<string>("DatabaseSettings:ConnectionString"))
    {
    }
    
    public async Task<Coupon> GetDiscount(string productName)
    {
        Coupon coupon = await QueryFirstOrDefaultAsync<Coupon>(DiscountRepositoryQueries.Get, new {ProductName = productName});
        return coupon ?? 
               new Coupon() {ProductName = "No Discount", Amount = 0, Description = "No Discount Description"};
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        int affected = await ExecuteAsync(DiscountRepositoryQueries.Create, 
            new 
            {
                ProductName = coupon.ProductName, 
                Description = coupon.Description,
                Amount = coupon.Amount
            });

        return affected != 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        int affected = await ExecuteAsync(DiscountRepositoryQueries.Update, 
            new 
            {
                ProductName = coupon.ProductName, 
                Description = coupon.Description,
                Amount = coupon.Amount
            });

        return affected != 0;    
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        int affected = await ExecuteAsync(DiscountRepositoryQueries.Delete, 
            new 
            {
                ProductName = productName 
            });

        return affected != 0;
    }
}
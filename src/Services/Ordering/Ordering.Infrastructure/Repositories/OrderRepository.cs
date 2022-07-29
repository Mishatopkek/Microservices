using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetOrderByUsername(string userName)
    {
        List<Order> orderList = await _context.Orders
            .Where(order => order.UserName == userName)
            .ToListAsync();
        return orderList;
    }
}
using Microsoft.EntityFrameworkCore;
using OnlineStore.Library.Common.Repos;
using OnlineStore.Library.Data;
using OnlineStore.Library.OrdersService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Library.OrdersService.Repos;

public class OrdersRepo : BaseRepo<Order>
{
    public OrdersRepo(OrdersDbContext context) : base(context)
    {
    }
    public override async Task<IEnumerable<Order>> GetAllAsync()
        => await Table.Include(nameof(Order.Articles)).ToListAsync();
    public override async Task<Order> GetOneAsync(Guid id)
        => await Task.Run(() => Table.Include(nameof(Order.Articles)).FirstOrDefault(entity => entity.Id == id));
}
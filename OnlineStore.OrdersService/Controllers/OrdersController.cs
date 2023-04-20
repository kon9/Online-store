using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Library.Common.Interfaces;
using OnlineStore.Library.Common.Repos;
using OnlineStore.Library.OrdersService.Models;

namespace OnlineStore.OrdersService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class OrdersController : BaseControllerRepo<Order>
    {
        public OrdersController(IRepo<Order> repo) : base(repo)
        {
        }

        protected override void UpdateProperties(Order source, Order destination)
        {
            destination.AddressId = source.AddressId;
            destination.UserId = source.UserId;
            destination.Articles = source.Articles;
            destination.UpdatedAt = DateTime.UtcNow;
        }
    }
}

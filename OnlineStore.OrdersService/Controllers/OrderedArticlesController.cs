using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Common.Interfaces;
using OnlineStore.Library.Common.Repos;

namespace OnlineStore.OrdersService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class OrderedArticlesController : BaseControllerRepo<OrderedArticle>
    {

        public OrderedArticlesController(IRepo<OrderedArticle> repo) : base(repo)
        {
        }

        protected override void UpdateProperties(OrderedArticle source, OrderedArticle destination)
        {
            destination.Name = source.Name;
            destination.Description = source.Description;

            if (destination.Price != source.Price)
            {
                destination.PriceListName = "Manually assigned";
            }

            destination.Price = source.Price;
            destination.Quantity = source.Quantity;
        }
    }
}

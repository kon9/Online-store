using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Common.Repos;
using OnlineStore.Library.Data;
using OnlineStore.Library.OrdersService.Models;

namespace OnlineStore.Library.OrdersService.Repos;

public class OrderedArticlesRepo:BaseRepo<OrderedArticle>
{
    public OrderedArticlesRepo(OrdersDbContext context) : base(context)
    {
    }
}
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Common.Repos;
using OnlineStore.Library.Data;

namespace OnlineStore.Library.ArticleService.Repos;

public class ArticlesRepo : BaseRepo<Article>
{
    public ArticlesRepo(OrdersDbContext context) : base(context)
    {
    }
}


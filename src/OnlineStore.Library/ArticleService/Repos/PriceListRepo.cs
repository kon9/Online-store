using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Common.Repos;
using OnlineStore.Library.Data;

namespace OnlineStore.Library.ArticleService.Repos;

public class PriceListRepo : BaseRepo<PriceList>
{
    public PriceListRepo(OrdersDbContext context) : base(context)
    {
    }
}
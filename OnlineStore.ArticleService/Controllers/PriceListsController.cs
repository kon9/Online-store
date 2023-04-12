using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Common.Interfaces;
using OnlineStore.Library.Common.Repos;

namespace OnlineStore.ArticlesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PriceListsController : BaseControllerRepo<PriceList>
    {
        public PriceListsController(IRepo<PriceList> repo) : base(repo)
        {
        }

        protected override void UpdateProperties(PriceList source, PriceList destination)
        {
            destination.Name = source.Name;
            destination.Price = source.Price;
            destination.ValidFrom = source.ValidFrom;
            destination.ValidTo = source.ValidTo;
        }
    }
}

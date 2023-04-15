using Microsoft.AspNetCore.Mvc;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Common.Interfaces;
using OnlineStore.Library.Common.Repos;

namespace OnlineStore.ArticlesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class ArticlesController : BaseControllerRepo<Article>
    {
        public ArticlesController(IRepo<Article> repo) : base(repo)
        {
        }

        protected override void UpdateProperties(Article source, Article destination)
        {
            destination.Name = source.Name;
            destination.Description = source.Description;
        }
    }
}

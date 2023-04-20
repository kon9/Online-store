using AutoFixture;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Clients.ArticlesService;

namespace OnlineStore.ArticlesService.ApiTests;

public class ArticlesRepoClientTests : BaseArticleServiceRepoApiTest<ArticlesClient, Article>
{
    protected override void CreateSystemUnderTests()
    {
        SystemUnderTests = new ArticlesClient(new HttpClient(), ServiceAddressOptions);
    }

    protected override void AssertObjectsAreEqual(Article expected, Article actual)
    {
        Assert.AreEqual(expected.Id, actual.Id);
        Assert.AreEqual(expected.Name, actual.Name);
        Assert.AreEqual(expected.Description, actual.Description);
    }

    protected override void AmendExpectedEntityForUpdating(Article expected)
    {
        expected.Name = Fixture.Create<string>();
        expected.Description = Fixture.Create<string>();
    }
}
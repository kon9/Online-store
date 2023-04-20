using AutoFixture;
using IdentityModel.Client;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Clients.ArticlesService;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace OnlineStore.ArticlesService.ApiTests;

public class PriceListsRepoClientTests : BaseArticleServiceRepoApiTest<PriceListsClient, PriceList>
{
    private ArticlesClient ArticlesClient;
    private Guid _articleId;

    [Fact]
    public override async Task GIVEN_Repo_Client_WHEN_I_add_entity_THEN_it_is_being_added_to_database()
    {
        var article = Fixture.Build<Article>().Create();

        var addArticleResponse = await ArticlesClient.Add(article);
        Assert.IsTrue(addArticleResponse.IsSuccessful);
        _articleId = article.Id;

        await base.GIVEN_Repo_Client_WHEN_I_add_entity_THEN_it_is_being_added_to_database();

        var removeArticleResponse = await ArticlesClient.Remove(article.Id);
        Assert.IsTrue(removeArticleResponse.IsSuccessful);
    }

    [Fact]
    public override async Task GIVEN_Repo_Client_WHEN_I_add_several_entities_THEN_it_is_being_added_to_database()
    {
        var article = Fixture.Build<Article>().Create();

        var addArticleResponse = await ArticlesClient.Add(article);
        Assert.IsTrue(addArticleResponse.IsSuccessful);
        _articleId = article.Id;

        await base.GIVEN_Repo_Client_WHEN_I_add_several_entities_THEN_it_is_being_added_to_database();

        var removeArticleResponse = await ArticlesClient.Remove(article.Id);
        Assert.IsTrue(removeArticleResponse.IsSuccessful);
    }

    [Fact]
    public override async Task GIVEN_Repo_Client_WHEN_I_update_entity_THEN_it_is_being_updated_in_database()
    {
        var article = Fixture.Build<Article>().Create();

        var addArticleResponse = await ArticlesClient.Add(article);
        Assert.IsTrue(addArticleResponse.IsSuccessful);
        _articleId = article.Id;

        await base.GIVEN_Repo_Client_WHEN_I_update_entity_THEN_it_is_being_updated_in_database();

        var removeArticleResponse = await ArticlesClient.Remove(article.Id);
        Assert.IsTrue(removeArticleResponse.IsSuccessful);
    }

    protected override void CreateSystemUnderTests()
    {
        SystemUnderTests = new PriceListsClient(new HttpClient(), ServiceAddressOptions);
        ArticlesClient = new ArticlesClient(new HttpClient(), ServiceAddressOptions);
    }

    protected override async Task AuthorizeSystemUnderTests()
    {
        var token = await AspIdentityClient.GetApiToken(AspIdentityApiOptions);
        SystemUnderTests.HttpClient.SetBearerToken(token.AccessToken);
        ArticlesClient.HttpClient.SetBearerToken(token.AccessToken);
    }

    protected override void AssertObjectsAreEqual(PriceList expected, PriceList actual)
    {
        Assert.AreEqual(expected.Name, actual.Name);
        Assert.AreEqual(expected.Price, actual.Price);
        Assert.AreEqual(expected.ValidFrom, actual.ValidFrom);
        Assert.AreEqual(expected.ValidTo, actual.ValidTo);
    }

    protected override void AmendExpectedEntityForUpdating(PriceList expected)
    {
        expected.Name = Fixture.Create<string>();
        expected.Price = Fixture.Create<decimal>();
        expected.ValidFrom = Fixture.Create<DateTime>();
        expected.ValidTo = Fixture.Create<DateTime>();
    }

    protected override PriceList CreateExpectedEntity() =>
        Fixture.Build<PriceList>().With(e => e.ArticleId, _articleId).Create();
}
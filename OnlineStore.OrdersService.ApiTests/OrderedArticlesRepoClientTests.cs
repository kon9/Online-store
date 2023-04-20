using AutoFixture;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Moq;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Clients.AspIdentity;
using OnlineStore.Library.Clients.OrdersService;
using OnlineStore.Library.Options;
using OnlineStore.Library.OrdersService.Models;
using Xunit;
using Assert = Xunit.Assert;

namespace OnlineStore.OrdersService.ApiTests;


public class OrderedArticlesRepoClientTests : IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private AspIdentityClient _identityServerClient;
    private OrdersClient _ordersClient;
    private OrderedArticlesClient _orderedArticlesClient;

    public async Task InitializeAsync()
    {
        ConfigureFixture();
        InitializeClients();
        await SetTokenForClients();
    }
    private void ConfigureFixture()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    private void InitializeClients()
    {
        var serviceAddressOptionsMock = new Mock<IOptions<ServiceAddressOptions>>();
        serviceAddressOptionsMock.Setup(m => m.Value).Returns(
            new ServiceAddressOptions { OrdersService = "https://localhost:7103", AspIdentityServer = "https://localhost:7202" });
        _ordersClient = new OrdersClient(new HttpClient(), serviceAddressOptionsMock.Object);
        _orderedArticlesClient = new OrderedArticlesClient(new HttpClient(), serviceAddressOptionsMock.Object);
        _identityServerClient = new AspIdentityClient(new HttpClient(), serviceAddressOptionsMock.Object);
    }

    private async Task SetTokenForClients()
    {
        var identityOptions = new AspIdentityApiOptions();
        var token = await _identityServerClient.GetApiToken(identityOptions);
        _orderedArticlesClient.HttpClient.SetBearerToken(token.AccessToken);
        _ordersClient.HttpClient.SetBearerToken(token.AccessToken);
    }

    [Fact]
    public async Task GIVEN_Ordered_Articles_Repo_Client_WHEN_I_add_article_THEN_it_is_being_added_to_database()
    {
        var order = _fixture.Build<Order>()
            .With(o => o.Articles, Enumerable.Empty<OrderedArticle>().ToList())
            .Create();

        var addOrderResponse = await _ordersClient.Add(order);
        Assert.True(addOrderResponse.IsSuccessful);

        var expected = _fixture.Build<OrderedArticle>()
            .With(oa => oa.Order, order)
            .With(oa => oa.OrderId, order.Id)
            .Create();

        var addOrderedArticleResponse = await _orderedArticlesClient.Add(expected);
        Assert.True(addOrderedArticleResponse.IsSuccessful);

        var getOneResponse = await _orderedArticlesClient.GetOne(addOrderedArticleResponse.Payload);
        Assert.True(getOneResponse.IsSuccessful);
        var actual = getOneResponse.Payload;

        AssertObjectsAreEqual(expected, actual);

        var removeOrderResponse = await _ordersClient.Remove(addOrderResponse.Payload);
        Assert.True(removeOrderResponse.IsSuccessful);
    }

    [Fact]
    public async Task
        GIVEN_Ordered_Articles_Repo_Client_WHEN_I_add_several_ordered_articles_THEN_it_is_being_added_to_database()
    {
        var order = _fixture.Build<Order>()
            .With(o => o.Articles, Enumerable.Empty<OrderedArticle>().ToList())
            .Create();

        var addOrderResponse = await _ordersClient.Add(order);
        Assert.True(addOrderResponse.IsSuccessful);

        var expected1 = _fixture.Build<OrderedArticle>()
            .With(oa => oa.Order, order)
            .With(oa => oa.OrderId, order.Id)
            .Create();

        var expected2 = _fixture.Build<OrderedArticle>()
            .With(oa => oa.Order, order)
            .With(oa => oa.OrderId, order.Id)
            .Create();

        var orderedArticlesToAdd = new[] { expected1, expected2 };

        var addOrderedArticleResponse = await _orderedArticlesClient.AddRange(orderedArticlesToAdd);
        Assert.True(addOrderedArticleResponse.IsSuccessful);

        var getAllResponse = await _orderedArticlesClient.GetAll();
        Assert.True(getAllResponse.IsSuccessful);
        var addedOrderedArticles = getAllResponse.Payload;

        foreach (var orderedArticleId in addOrderedArticleResponse.Payload)
        {
            var expectedOrder = orderedArticlesToAdd.Single(o => o.Id == orderedArticleId);
            var actualOrder = addedOrderedArticles.Single(o => o.Id == orderedArticleId);
            AssertObjectsAreEqual(expectedOrder, actualOrder);
        }

        var removeRangeResponse = await _orderedArticlesClient.RemoveRange(addOrderedArticleResponse.Payload);
        Assert.True(removeRangeResponse.IsSuccessful);

        var removeOrderResponse = await _ordersClient.Remove(order.Id);
        Assert.True(removeOrderResponse.IsSuccessful);
    }

    [Fact]
    public async Task
        GIVEN_Ordered_Articles_Repo_Client_WHEN_I_update_ordered_article_THEN_it_is_being_update_in_database()
    {
        var order = _fixture.Build<Order>()
            .With(o => o.Articles, Enumerable.Empty<OrderedArticle>().ToList())
            .Create();

        var addOrderResponse = await _ordersClient.Add(order);
        Assert.True(addOrderResponse.IsSuccessful);

        var expected = _fixture.Build<OrderedArticle>()
            .With(oa => oa.Order, order)
            .With(oa => oa.OrderId, order.Id)
            .Create();

        var addOrderedArticleResponse = await _orderedArticlesClient.Add(expected);
        Assert.True(addOrderedArticleResponse.IsSuccessful);

        expected.Name = _fixture.Create<string>();
        expected.Description = _fixture.Create<string>();
        expected.Price = _fixture.Create<decimal>();
        expected.Quantity = _fixture.Create<int>();

        var updateResponse = await _orderedArticlesClient.Update(expected);
        Assert.True(updateResponse.IsSuccessful);
        var actual = updateResponse.Payload;

        AssertObjectsAreEqual(expected, actual);

        var removeOrderResponse = await _ordersClient.Remove(addOrderResponse.Payload);
        Assert.True(removeOrderResponse.IsSuccessful);
    }

    private static void AssertObjectsAreEqual(OrderedArticle expected, OrderedArticle actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.Price, actual.Price);
        Assert.Equal(expected.Quantity, actual.Quantity);

        if (expected.Price != actual.Price) expected.PriceListName = "Manually assigned";
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
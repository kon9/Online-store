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

public class OrdersRepoClientTests : IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private AspIdentityClient _identityServerClient;
    private OrdersClient _oredersClient;

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
        serviceAddressOptionsMock.Setup(m => m.Value).Returns(new ServiceAddressOptions
        { OrdersService = "https://localhost:7103", AspIdentityServer = "https://localhost:7202" });
        _oredersClient = new OrdersClient(new HttpClient(), serviceAddressOptionsMock.Object);
        _identityServerClient = new AspIdentityClient(new HttpClient(), serviceAddressOptionsMock.Object);
    }

    private async Task SetTokenForClients()
    {
        var identityOptions = new AspIdentityApiOptions();
        var token = await _identityServerClient.GetApiToken(identityOptions);
        _oredersClient.HttpClient.SetBearerToken(token.AccessToken);
    }

    [Fact]
    public async Task GIVEN_Orders_Repo_Client_WHEN_I_add_order_THEN_it_is_being_added_to_database()
    {
        var expected = _fixture.Build<Order>()
            .With(o => o.Articles, _fixture.CreateMany<OrderedArticle>().ToList())
            .Create();

        var addResponse = await _oredersClient.Add(expected);
        Assert.True(addResponse.IsSuccessful);

        var getOneResponse = await _oredersClient.GetOne(addResponse.Payload);
        Assert.True(getOneResponse.IsSuccessful);
        var actual = getOneResponse.Payload;

        AssertObjectsAreEqual(expected, actual);

        var removeResponse = await _oredersClient.Remove(addResponse.Payload);
        Assert.True(removeResponse.IsSuccessful);
    }

    [Fact]
    public async Task GIVEN_Orders_Repo_Client_WHEN_I_add_several_orders_THEN_it_is_being_added_to_database()
    {
        var expected1 = _fixture.Build<Order>()
            .With(o => o.Articles, _fixture.CreateMany<OrderedArticle>().ToList())
            .Create();

        var expected2 = _fixture.Build<Order>()
            .With(o => o.Articles, _fixture.CreateMany<OrderedArticle>().ToList())
            .Create();

        var ordersToAdd = new[] { expected1, expected2 };

        var addRangeResponse = await _oredersClient.AddRange(ordersToAdd);
        Assert.True(addRangeResponse.IsSuccessful);

        var getAllResponse = await _oredersClient.GetAll();
        Assert.True(getAllResponse.IsSuccessful);

        var addedOrders = getAllResponse.Payload;

        foreach (var orderId in addRangeResponse.Payload)
        {
            var expectedOrder = ordersToAdd.Single(o => o.Id == orderId);
            var actualOrder = addedOrders.Single(o => o.Id == orderId);
            AssertObjectsAreEqual(expectedOrder, actualOrder);
        }

        var removeRangeResponse = await _oredersClient.RemoveRange(addRangeResponse.Payload);
        Assert.True(removeRangeResponse.IsSuccessful);
    }

    [Fact]
    public async Task GIVEN_Orders_Repo_Client_WHEN_I_update_order_THEN_it_is_being_update_in_database()
    {
        var orderedArticles = _fixture.CreateMany<OrderedArticle>().ToList();

        var expected = _fixture.Build<Order>()
            .With(o => o.Articles, orderedArticles)
            .Create();

        var addResponse = await _oredersClient.Add(expected);
        Assert.True(addResponse.IsSuccessful);

        orderedArticles.ForEach(oa => oa.Name = _fixture.Create<string>());

        expected.UserId = _fixture.Create<Guid>();
        expected.AddressId = _fixture.Create<Guid>();
        expected.Articles = orderedArticles;

        var updateResponse = await _oredersClient.Update(expected);
        Assert.True(updateResponse.IsSuccessful);
        var actual = updateResponse.Payload;

        AssertObjectsAreEqual(expected, actual);

        var removeResponse = await _oredersClient.Remove(addResponse.Payload);
        Assert.True(removeResponse.IsSuccessful);
    }

    private static void AssertObjectsAreEqual(Order expected, Order actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.AddressId, actual.AddressId);
        Assert.Equal(expected.UserId, actual.UserId);
        Assert.Equal(expected.CreatedAt, actual.CreatedAt);
        Assert.Equal(expected.Articles.Count(), actual.Articles.Count());
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
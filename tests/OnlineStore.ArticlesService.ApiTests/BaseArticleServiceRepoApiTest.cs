using AutoFixture;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Moq;
using OnlineStore.Library.Clients;
using OnlineStore.Library.Clients.AspIdentity;
using OnlineStore.Library.Common.Interfaces;
using OnlineStore.Library.Options;
using Xunit;
using Assert = Xunit.Assert;

namespace OnlineStore.ArticlesService.ApiTests;

public abstract class BaseArticleServiceRepoApiTest<TClient, TEntity> : IAsyncLifetime
        where TClient : IRepoClient<TEntity>, IHttpClientContainer
        where TEntity : IIdentifiable
{
    protected readonly Fixture Fixture = new Fixture();
    protected IOptions<ServiceAddressOptions> ServiceAddressOptions;
    protected AspIdentityApiOptions AspIdentityApiOptions;
    protected AspIdentityClient AspIdentityClient;
    protected TClient SystemUnderTests;

    public async Task InitializeAsync()
    {
        ConfigureFixture();
        SetServiceAddressOptions();
        SetIdentityServerApiOptions();
        AspIdentityClient = new AspIdentityClient(new HttpClient(), ServiceAddressOptions);
        CreateSystemUnderTests();
        await AuthorizeSystemUnderTests();
    }

    protected virtual void ConfigureFixture()
    {
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }


    protected virtual void SetServiceAddressOptions()
    {
        var serviceAddressOptionsMock = new Mock<IOptions<ServiceAddressOptions>>();
        serviceAddressOptionsMock.Setup(m => m.Value).Returns(new ServiceAddressOptions()
        { ArticlesService = "https://localhost:7289", AspIdentityServer = "https://localhost:7202" });
        ServiceAddressOptions = serviceAddressOptionsMock.Object;
    }

    protected virtual void SetIdentityServerApiOptions()
    {
        AspIdentityApiOptions = new AspIdentityApiOptions();
    }

    protected virtual async Task AuthorizeSystemUnderTests()
    {
        var token = await AspIdentityClient.GetApiToken(AspIdentityApiOptions);
        SystemUnderTests.HttpClient.SetBearerToken(token.AccessToken);
    }

    /// <summary>
    /// Creates an instance of type TClient.
    /// </summary>
    protected abstract void CreateSystemUnderTests();

    [Fact]
    public virtual async Task GIVEN_Repo_Client_WHEN_I_add_entity_THEN_it_is_being_added_to_database()
    {
        var expected = CreateExpectedEntity();

        var addResponse = await SystemUnderTests.Add(expected);
        Assert.True(addResponse.IsSuccessful);

        var getOneResponse = await SystemUnderTests.GetOne(addResponse.Payload);
        Assert.True(getOneResponse.IsSuccessful);
        var actual = getOneResponse.Payload;

        AssertObjectsAreEqual(expected, actual);

        var removeResponse = await SystemUnderTests.Remove(addResponse.Payload);
        Assert.True(removeResponse.IsSuccessful);
    }

    [Fact]
    public virtual async Task GIVEN_Repo_Client_WHEN_I_add_several_entities_THEN_it_is_being_added_to_database()
    {
        var expected1 = CreateExpectedEntity();
        var expected2 = CreateExpectedEntity();

        var entitiesToAdd = new[] { expected1, expected2 };

        var addRangeResponse = await SystemUnderTests.AddRange(entitiesToAdd);
        Assert.True(addRangeResponse.IsSuccessful);

        var getAllResponse = await SystemUnderTests.GetAll();
        Assert.True(getAllResponse.IsSuccessful);

        var addedEntities = getAllResponse.Payload;

        foreach (var entityId in addRangeResponse.Payload)
        {
            var expectedEntity = entitiesToAdd.Single(o => o.Id == entityId);
            var actualEntity = addedEntities.Single(o => o.Id == entityId);
            AssertObjectsAreEqual(expectedEntity, actualEntity);
        }

        var removeRangeResponse = await SystemUnderTests.RemoveRange(addRangeResponse.Payload);
        Assert.True(removeRangeResponse.IsSuccessful);
    }

    [Fact]
    public virtual async Task GIVEN_Repo_Client_WHEN_I_update_entity_THEN_it_is_being_updated_in_database()
    {
        var expected = CreateExpectedEntity();

        var addResponse = await SystemUnderTests.Add(expected);
        Assert.True(addResponse.IsSuccessful);

        AmendExpectedEntityForUpdating(expected);

        var updateResponse = await SystemUnderTests.Update(expected);
        Assert.True(updateResponse.IsSuccessful);
        var actual = updateResponse.Payload;

        AssertObjectsAreEqual(expected, actual);

        var removeResponse = await SystemUnderTests.Remove(addResponse.Payload);
        Assert.True(removeResponse.IsSuccessful);
    }

    protected virtual TEntity CreateExpectedEntity() => Fixture.Build<TEntity>().Create();
    protected abstract void AssertObjectsAreEqual(TEntity expected, TEntity actual);
    protected abstract void AmendExpectedEntityForUpdating(TEntity expected);


    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

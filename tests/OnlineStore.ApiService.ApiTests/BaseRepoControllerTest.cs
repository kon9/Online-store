using AutoFixture;

namespace OnlineStore.ApiService.ApiTests;

public class BaseRepoControllerTests
{
    protected readonly Fixture Fixture = new Fixture();
    protected IdentityServerClient IdentityServerClient;
    protected HttpClient SystemUnderTests;

    public BaseRepoControllerTests()
    {
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [SetUp]
    public async Task Setup()
    {
        var serviceAdressOptionsMock = new Mock<IOptions<ServiceAdressOptions>>();
        serviceAdressOptionsMock.Setup(m => m.Value).Returns(new ServiceAdressOptions()
        {
            OrdersService = "https://localhost:5005",
            IdentityServer = "https://localhost:5001",
            ApiService = "https://localhost:5009"
        });

        SystemUnderTests = new HttpClient() { BaseAddress = new Uri(serviceAdressOptionsMock.Object.Value.ApiService) };
        IdentityServerClient = new IdentityServerClient(new HttpClient(), serviceAdressOptionsMock.Object);

        var token = await IdentityServerClient.GetApiToken(new IdentityServerUserNamePassword()
        {
            UserName = "andrey",
            Password = "Pass_123"
        });

        SystemUnderTests.SetBearerToken(token.AccessToken);
    }
}
using Microsoft.Extensions.Options;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Options;
using System;
using System.Net.Http;

namespace OnlineStore.Library.Clients.ArticlesService;

public class PriceListsClient : RepoClient<PriceList>
{
    public PriceListsClient(HttpClient client, IOptions<ServiceAddressOptions> options) : base(client, options)
    { }

    protected override void InitializeClient(IOptions<ServiceAddressOptions> options)
    {
        HttpClient.BaseAddress = new Uri(options.Value.ArticlesService);
    }

    protected override void SetControllerName()
    {
        ControllerName = "PriceLists";
    }
}
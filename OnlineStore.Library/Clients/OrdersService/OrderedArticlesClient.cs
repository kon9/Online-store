using Microsoft.Extensions.Options;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.Options;
using System;
using System.Net.Http;

namespace OnlineStore.Library.Clients.OrdersService;

public class OrderedArticlesClient : BaseClientRepo<OrderedArticle>
{
    public OrderedArticlesClient(HttpClient client, IOptions<ServiceAddressOptions> options) : base(client, options)
    { }

    protected override void InitializeClient(IOptions<ServiceAddressOptions> options)
    {
        HttpClient.BaseAddress = new Uri(options.Value.OrdersService);
    }

    protected override void SetControllerName()
    {
        ControllerName = "OrderedArticles";
    }
}
using Microsoft.Extensions.Options;
using OnlineStore.Library.Options;
using OnlineStore.Library.OrdersService.Models;
using System;
using System.Net.Http;

namespace OnlineStore.Library.Clients.OrdersService;

public class OrdersClient : RepoClient<Order>
{
    public OrdersClient(HttpClient client, IOptions<ServiceAddressOptions> options) : base(client, options)
    { }

    protected override void InitializeClient(IOptions<ServiceAddressOptions> options)
    {
        HttpClient.BaseAddress = new Uri(options.Value.OrdersService);
    }

    protected override void SetControllerName()
    {
        ControllerName = "Orders";
    }
}
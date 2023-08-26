﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.ApiService.Authorization;
using OnlineStore.Library.Clients;
using OnlineStore.Library.Constants;
using OnlineStore.Library.OrdersService.Models;

namespace OnlineStore.ApiService.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class OrdersController : ControllerWithClientAuthorization<IRepoClient<Order>>
{
    public OrdersController(IRepoClient<Order> client, IClientAuthorization clientAuthorization) : base(client, clientAuthorization)
    { }

    [HttpPost(RepoActions.Add)]
    public async Task<ActionResult> Add([FromBody] Order entity)
    {
        var response = await Client.Add(entity);
        return Ok(response.Payload);
    }

    [HttpPost(RepoActions.AddRange)]
    public async Task<ActionResult> Add([FromBody] IEnumerable<Order> entities)
    {
        var response = await Client.AddRange(entities);
        return Ok(response.Payload);
    }

    [HttpGet]
    public async Task<ActionResult> GetOne(Guid id)
    {
        var response = await Client.GetOne(id);
        return Ok(response.Payload);
    }

    [HttpGet(RepoActions.GetAll)]
    public async Task<ActionResult> GetAll()
    {
        var response = await Client.GetAll();
        return Ok(response.Payload);
    }

    [HttpPost(RepoActions.Remove)]
    public virtual async Task<ActionResult> Remove([FromBody] Guid id)
    {
        await Client.Remove(id);
        return NoContent();
    }

    [HttpPost(RepoActions.RemoveRange)]
    public virtual async Task<ActionResult> Remove([FromBody] IEnumerable<Guid> ids)
    {
        await Client.RemoveRange(ids);
        return NoContent();
    }

    [HttpPost(RepoActions.Update)]
    public virtual async Task<ActionResult> Update([FromBody] Order entity)
    {
        var response = await Client.Update(entity);
        return Ok(response.Payload);
    }
}

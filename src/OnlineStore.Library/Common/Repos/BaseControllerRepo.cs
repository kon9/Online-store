using Microsoft.AspNetCore.Mvc;
using OnlineStore.Library.Common.Interfaces;
using OnlineStore.Library.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Library.Common.Repos;

public abstract class BaseControllerRepo<T> : ControllerBase where T : class, IIdentifiable
{
    protected readonly IRepo<T> Repo;

    public BaseControllerRepo(IRepo<T> repo)
    {
        Repo = repo;
    }

    [HttpPost(RepoActions.Add)]
    public async Task<ActionResult> Add([FromBody] T entity)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values);

        var articleId = await Repo.AddAsync(entity);
        return Ok(articleId);
    }

    [HttpPost(RepoActions.AddRange)]
    public async Task<ActionResult> Add([FromBody] IEnumerable<T> entities)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values);

        var articleIds = await Repo.AddRangeAsync(entities);
        return Ok(articleIds);
    }

    [HttpGet]
    public async Task<ActionResult> GetOne(Guid id)
    {
        var article = await Repo.GetOneAsync(id);
        return Ok(article);
    }

    [HttpGet(RepoActions.GetAll)]
    public async Task<ActionResult> GetAll()
    {
        var entities = await Repo.GetAllAsync();
        return Ok(entities);
    }

    [HttpPost(RepoActions.Remove)]
    public virtual async Task<ActionResult> Remove([FromBody] Guid id)
    {
        await Repo.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost(RepoActions.RemoveRange)]
    public virtual async Task<ActionResult> Remove([FromBody] IEnumerable<Guid> ids)
    {
        await Repo.DeleteRangeAsync(ids);
        return NoContent();
    }

    [HttpPost(RepoActions.Update)]
    public virtual async Task<ActionResult> Update([FromBody] T entity)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values);

        var entityToBeUpdated = await Repo.GetOneAsync(entity.Id);
        if (entityToBeUpdated == null) return BadRequest($"Entity with id = {entity.Id} was not found.");

        UpdateProperties(entity, entityToBeUpdated);

        await Repo.SaveAsync(entityToBeUpdated);
        return Ok(entityToBeUpdated);
    }

    protected abstract void UpdateProperties(T source, T destination);
}
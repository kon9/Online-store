using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnlineStore.Library.Common.Responses;
using OnlineStore.Library.Constants;
using OnlineStore.Library.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Library.Clients;


public abstract class BaseClientRepo<T> : IBaseClientRepo, IHttpClientContainer
{
    protected BaseClientRepo(HttpClient client, IOptions<ServiceAddressOptions> options)
    {
        HttpClient = client;
        InitializeClient(options);
        SetControllerName();
    }

    protected abstract void InitializeClient(IOptions<ServiceAddressOptions> options);

    protected abstract void SetControllerName();

    public HttpClient HttpClient { get; init; }

    protected string ControllerName { get; set; }

    public async Task<ServiceResponse<Guid>> Add(T entity)
    {
        return await SendPostRequestAsync<T, Guid>(entity, $"{ControllerName}/{RepoActions.Add}");
    }

    public async Task<ServiceResponse<IEnumerable<Guid>>> AddRange(IEnumerable<T> entities)
    {
        return await SendPostRequestAsync<IEnumerable<T>, IEnumerable<Guid>>(entities,
            $"{ControllerName}/{RepoActions.AddRange}");
    }

    public async Task<ServiceResponse<T>> Update(T entity)
    {
        return await SendPostRequestAsync<T, T>(entity, $"{ControllerName}/{RepoActions.Update}");
    }

    public async Task<ServiceResponse<object>> Remove(Guid entityId)
    {
        return await SendPostRequestAsync<Guid, object>(entityId, $"{ControllerName}/{RepoActions.Remove}");
    }

    public async Task<ServiceResponse<object>> RemoveRange(IEnumerable<Guid> entityIds)
    {
        return await SendPostRequestAsync<IEnumerable<Guid>, object>(entityIds,
            $"{ControllerName}/{RepoActions.RemoveRange}");
    }

    public async Task<ServiceResponse<T>> GetOne(Guid entityId)
    {
        return await SendGetRequestAsync<T>($"{ControllerName}?Id={entityId}");
    }

    public async Task<ServiceResponse<IEnumerable<T>>> GetAll()
    {
        return await SendGetRequestAsync<IEnumerable<T>>($"{ControllerName}/{RepoActions.GetAll}");
    }

    protected async Task<ServiceResponse<TResponse>> SendPostRequestAsync<TRequest, TResponse>(TRequest request, string path)
    {
        var jsonContent = JsonConvert.SerializeObject(request);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var requestResult = await HttpClient.PostAsync(path, httpContent);

        requestResult.EnsureSuccessStatusCode();

        var responseJson = await requestResult.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<TResponse>(responseJson);
        return new ServiceResponse<TResponse>(response);
    }

    protected async Task<ServiceResponse<TResponse>> SendGetRequestAsync<TResponse>(string path)
    {
        var requestResult = await HttpClient.GetAsync(path);

        requestResult.EnsureSuccessStatusCode();

        var responseJson = await requestResult.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<TResponse>(responseJson);
        return new ServiceResponse<TResponse>(response);
    }
}

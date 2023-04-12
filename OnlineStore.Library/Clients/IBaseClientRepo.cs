using OnlineStore.Library.Common.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Library.Clients;

public interface IBaseClientRepo
{
    public interface IRepoClient<T>
    {
        Task<ServiceResponse<Guid>> Add(T entity);

        Task<ServiceResponse<IEnumerable<Guid>>> AddRange(IEnumerable<T> entities);

        Task<ServiceResponse<T>> Update(T entity);

        Task<ServiceResponse<object>> Remove(Guid entityId);

        Task<ServiceResponse<object>> RemoveRange(IEnumerable<Guid> entityIds);

        Task<ServiceResponse<T>> GetOne(Guid entityId);

        Task<ServiceResponse<IEnumerable<T>>> GetAll();
    }
}
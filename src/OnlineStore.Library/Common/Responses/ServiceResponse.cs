﻿using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Library.Common.Responses;

public class ServiceResponse<T>
{
    public ServiceResponse(T payload)
    {
        Payload = payload;
    }

    public ServiceResponse(ICollection<string> errors)
    {
        Errors = errors;
    }

    public T Payload { get; init; } = default;
    public ICollection<string> Errors { get; init; } = new List<string>();
    public bool IsSuccessful => !Errors.Any();
}

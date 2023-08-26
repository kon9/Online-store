using System;

namespace OnlineStore.Library.Common.Interfaces
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}

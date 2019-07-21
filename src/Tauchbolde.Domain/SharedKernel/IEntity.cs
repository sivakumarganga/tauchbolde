using System;
using System.Collections.Generic;

namespace Tauchbolde.Domain.SharedKernel
{
    public interface IEntity
    {
        Guid Id { get; set; }
        IList<DomainEventBase> Events { get; }
    }
}
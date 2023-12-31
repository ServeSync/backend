﻿using ServeSync.Domain.SeedWorks.Models.Interfaces;

namespace ServeSync.Domain.SeedWorks.Repositories;

public interface IRepository<TAggregateRoot, TKey> : IReadOnlyRepository<TAggregateRoot, TKey>, ICommandRepository<TAggregateRoot, TKey> 
    where TAggregateRoot : class, IAggregateRoot<TKey>
    where TKey : IEquatable<TKey>
{
    
}

public interface IRepository<TAggregateRoot> : IRepository<TAggregateRoot, Guid>
    where TAggregateRoot : class, IAggregateRoot
{
    
}
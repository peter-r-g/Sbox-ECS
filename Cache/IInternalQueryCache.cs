using EntityComponentSystem.Queries;
using EntityComponentSystem.Systems;
using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Cache;

/// <summary>
/// An internally facing interface to a query cache implementation.
/// </summary>
internal interface IInternalQueryCache : IQueryCache
{
	/// <summary>
	/// Gets the cached query of a system. If there is nothing cached, then a new one is created and returned.
	/// </summary>
	/// <typeparam name="TSystem">The type of the system being passed.</typeparam>
	/// <typeparam name="TEntity">The type of entity that the system uses.</typeparam>
	/// <param name="system">The system to get the cache of.</param>
	/// <param name="entities">The entities to create a new <see cref="Query{TEntity}"/> around if nothing is cached.</param>
	/// <returns>The cached query of a system. If there is nothing cached, then a new one is created and returned.</returns>
	Query<TEntity> GetOrCache<TSystem, TEntity>( TSystem system, IEnumerable<TEntity> entities )
		where TSystem : ISystem<TEntity>
		where TEntity : IEntity;
}

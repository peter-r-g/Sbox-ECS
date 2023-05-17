using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Cache;

/// <summary>
/// A publically facing interface to a query cache implementation.
/// </summary>
public interface IQueryCache
{
	/// <summary>
	/// Invalidates the entire query cache.
	/// </summary>
	void Invalidate();

	/// <summary>
	/// Invalidates the cache for a specific system.
	/// </summary>
	/// <param name="system">The system whose cache to invalidate.</param>
	void InvalidateFor( IComponent system );

	/// <summary>
	/// Invalidates the cache for all queries using the underlying collection.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity that is contained in the collection.</typeparam>
	/// <param name="collection">The collection to check for in queries.</param>
	void InvalidateForCollection<TEntity>( IEnumerable<TEntity> collection )
		where TEntity : IEntity;
}

using EntityComponentSystem.Systems;
using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Cache;

/// <summary>
/// An implementation of the <see cref="IInternalQueryCache"/> that respects if a system actually wants caching.
/// </summary>
internal sealed class QueryCache : IInternalQueryCache
{
	/// <summary>
	/// A dictionary containing all of the cached queries.
	/// </summary>
	private readonly Dictionary<IComponent, object> queryCache = new();

	/// <inheritdoc/>
	public Query<TEntity> GetOrCache<TSystem, TEntity>( TSystem system, IEnumerable<TEntity> entities )
		where TSystem : ISystem<TEntity>
		where TEntity : IEntity
	{
		if ( queryCache.TryGetValue( system, out var cachedQuery ) )
			return (Query<TEntity>)cachedQuery;

		var query = QueryBuilder.From( entities );
		system.FilterEntities( query );
		if ( system.SupportsCaching )
			queryCache.Add( system, query );

		return query;
	}

	/// <inheritdoc/>
	public void Invalidate()
	{
		queryCache.Clear();
	}

	/// <inheritdoc/>
	public void InvalidateFor( IComponent system )
	{
		queryCache.Remove( system );
	}
}

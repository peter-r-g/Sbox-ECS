using EntityComponentSystem.Systems;
using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Cache;

/// <summary>
/// An implementation of the <see cref="IInternalQueryCache"/> where nothing is cached.
/// </summary>
internal sealed class FakeQueryCache : IInternalQueryCache
{
	/// <inheritdoc/>
	public Query<TEntity> GetOrCache<TSystem, TEntity>( TSystem system, IEnumerable<TEntity> entities )
		where TSystem : ISystem<TEntity>
		where TEntity : IEntity
	{
		var query = QueryBuilder.From( entities );
		system.FilterEntities( query );
		return query;
	}

	/// <inheritdoc/>
	public void Invalidate()
	{
	}

	/// <inheritdoc/>
	public void InvalidateFor( IComponent system )
	{
	}
}

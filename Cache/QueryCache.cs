﻿using EntityComponentSystem.Queries;
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
	private readonly Dictionary<IComponent, IInternalQuery> queryCache = new();

	/// <inheritdoc/>
	public Query<TEntity> GetOrCache<TSystem, TEntity>( TSystem system, IEnumerable<TEntity> entities )
		where TSystem : ISystem<TEntity>
		where TEntity : IEntity
	{
		if ( queryCache.TryGetValue( system, out var queryObj ) )
		{
			if ( queryObj is Query<TEntity> cachedQuery )
				return cachedQuery;

			queryCache.Remove( system );

			if ( ECS.Instance!.Configuration.IsLoggerEnabled( Logs.UnexpectedCacheMiss ) )
				ECS.Logger!.Warning( $"Unexpected cache miss for \"{system.GetType().Name}\", did you change the input entity type?" );
		}

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

	/// <inheritdoc/>
	public void InvalidateForCollection<TEntity>( IEnumerable<TEntity> collection )
		where TEntity : IEntity
	{
		var cachesToRemove = new HashSet<IComponent>();

		foreach ( var (system, query) in queryCache )
		{
			if ( ReferenceEquals( query.Entities, collection ) )
				cachesToRemove.Add( system );
		}

		foreach ( var system in cachesToRemove )
			queryCache.Remove( system );
	}
}

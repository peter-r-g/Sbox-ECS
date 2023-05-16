using EntityComponentSystem.Extensions;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentSystem;

public sealed class Query<TEntity> where TEntity : IEntity
{
	public IEnumerable<TEntity> Output => CurrentEntities;

	private bool UseEnumerable { get; }

	private IEnumerable<TEntity>? Entities { get; }

	private Func<IEnumerable<TEntity>>? EntitiesGetter { get; }

	private List<TEntity> CurrentEntities { get; set; } = new();
		 
	internal Query( IEnumerable<TEntity> entities )
	{
		UseEnumerable = true;
		Entities = entities;

		Reset();
	}

	internal Query( Func<IEnumerable<TEntity>> entitiesGetter )
	{
		UseEnumerable = false;
		EntitiesGetter = entitiesGetter;

		Reset();
	}

	public Query<TEntity> WithComponent<TComponent>( Func<TComponent, bool>? cb = null ) where TComponent : IComponent
	{
		for ( var i = 0; i < CurrentEntities.Count; i++ )
		{
			if ( !CurrentEntities[i].Components.TryGet<TComponent>( out var component ) )
			{
				CurrentEntities.RemoveAt( i );
				i--;
				continue;
			}

			if ( cb is not null && !cb( component ) )
			{
				CurrentEntities.RemoveAt( i );
				i--;
				continue;
			}
		}

		return this;
	}

	public Query<TEntity> WithoutComponent<TComponent>() where TComponent : IComponent
	{
		for ( var i = 0; i < CurrentEntities.Count; i++ )
		{
			if ( !CurrentEntities[i].Components.TryGet<TComponent>( out _ ) )
				continue;

			CurrentEntities.RemoveAt( i );
			i--;
		}

		return this;
	}

	public Query<TEntity> WithTag( ReadOnlySpan<char> tag )
	{
		for ( var i = 0; i < CurrentEntities.Count; i++ )
		{
			if ( CurrentEntities[i].HasTag( tag ) )
				continue;

			CurrentEntities.RemoveAt( i );
			i--;
		}

		return this;
	}

	public Query<TEntity> WithoutTag( ReadOnlySpan<char> tag )
	{
		for ( var i = 0; i < CurrentEntities.Count; i++ )
		{
			if ( !CurrentEntities[i].HasTag( tag ) )
				continue;

			CurrentEntities.RemoveAt( i );
			i--;
		}

		return this;
	}

	public Query<TEntity> Reset()
	{
		CurrentEntities = !UseEnumerable ? EntitiesGetter!().ToList() : Entities!.ToList();

		return this;
	}
}

public static class QueryBuilder
{
	public static Query<T> From<T>( IEnumerable<T> entities ) where T : IEntity
	{
		return new Query<T>( entities );
	}

	public static Query<T> From<T>( Func<IEnumerable<T>> entitiesGetter ) where T : IEntity
	{
		return new Query<T>( entitiesGetter );
	}
}

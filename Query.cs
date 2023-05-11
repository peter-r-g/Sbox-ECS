using ParkourPainters.Extensions;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ECS;

public sealed class Query<TEntity> where TEntity : IEntity
{
	public IEnumerable<TEntity> Output => CurrentEntities;

	[MemberNotNullWhen( true, nameof( Entities ) )]
	[MemberNotNullWhen( false, nameof( EntitiesGetter ) )]
	private bool UseEnumerable { get; }

	private IEnumerable<TEntity>? Entities { get; }

	private Func<IEnumerable<TEntity>>? EntitiesGetter { get; }

	private List<TEntity> CurrentEntities { get; set; } = new();
		 
	private Query( IEnumerable<TEntity> entities )
	{
		UseEnumerable = true;
		Entities = entities;

		Reset();
	}

	private Query( Func<IEnumerable<TEntity>> entitiesGetter )
	{
		UseEnumerable = false;
		EntitiesGetter = entitiesGetter;

		Reset();
	}

	public Query<TEntity> WithComponent<TComponent>() where TComponent : IComponent
	{
		for ( var i = 0; i < CurrentEntities.Count; i++ )
		{
			if ( CurrentEntities[i].Components.TryGet<TComponent>( out _ ) )
				continue;

			CurrentEntities.RemoveAt( i );
			i--;
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
		CurrentEntities = !UseEnumerable ? EntitiesGetter().ToList() : Entities.ToList();

		return this;
	}

	public static Query<T> From<T>( IEnumerable<T> entities ) where T : TEntity
	{
		return new Query<T>( entities );
	}

	public static Query<T> From<T>( Func<IEnumerable<T>> entitiesGetter ) where T : TEntity
	{
		return new Query<T>( entitiesGetter );
	}
}

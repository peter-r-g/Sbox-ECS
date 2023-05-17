using EntityComponentSystem.Extensions;
using Sandbox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentSystem;

/// <summary>
/// Encapsulates a filter to query a sequence of entities.
/// </summary>
/// <typeparam name="TEntity">The type of entities in the sequence.</typeparam>
public sealed class Query<TEntity> : IEnumerable<TEntity> where TEntity : IEntity
{
	/// <summary>
	/// The sequence of entities to filter.
	/// </summary>
	private IEnumerable<TEntity> Entities { get; }
	/// <summary>
	/// A boolean array of indices to ignore in <see cref="Entities"/>.
	/// </summary>
	private BitArray IndicesToIgnore { get; }
	
	/// <summary>
	/// Initializes an instance of <see cref="Query{TEntity}"/> with a sequence of entities to query.
	/// </summary>
	/// <param name="entities">The sequence of entities to query.</param>
	internal Query( IEnumerable<TEntity> entities )
	{
		Entities = entities;
		IndicesToIgnore = new BitArray( entities.Count() );
	}

	/// <summary>
	/// Filters the sequence of entities to what has <see ref="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to filter by.</typeparam>
	/// <returns>The same instance of <see cref="Query{TEntity}"/>.</returns>
	public Query<TEntity> WithComponent<TComponent>() where TComponent : IComponent
	{
		using var enumerator = Entities.GetEnumerator();
		var index = -1;

		while ( enumerator.MoveNext() )
		{
			var entity = enumerator.Current;
			index++;

			if ( IndicesToIgnore[index] )
				continue;

			if ( entity.Components.TryGet<TComponent>( out var component ) )
				continue;

			IndicesToIgnore[index] = true;
		}

		return this;
	}

	/// <summary>
	/// Filters the sequence of entities to what does not have <see ref="TComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to filter by.</typeparam>
	/// <returns>The same instance of <see cref="Query{TEntity}"/>.</returns>
	public Query<TEntity> WithoutComponent<TComponent>() where TComponent : IComponent
	{
		using var enumerator = Entities.GetEnumerator();
		var index = -1;

		while ( enumerator.MoveNext() )
		{
			var entity = enumerator.Current;
			index++;

			if ( IndicesToIgnore[index] )
				continue;

			if ( !entity.Components.TryGet<TComponent>( out _ ) )
				continue;

			IndicesToIgnore[index] = true;
		}

		return this;
	}

	/// <summary>
	/// Filters the sequence of entities to what has the tag.
	/// </summary>
	/// <param name="tag">The tag to filter by.</param>
	/// <returns>The same instance of <see cref="Query{TEntity}"/>.</returns>
	public Query<TEntity> WithTag( ReadOnlySpan<char> tag )
	{
		using var enumerator = Entities.GetEnumerator();
		var index = -1;

		while ( enumerator.MoveNext() )
		{
			var entity = enumerator.Current;
			index++;

			if ( IndicesToIgnore[index] )
				continue;

			if ( entity.HasTag( tag ) )
				continue;

			IndicesToIgnore[index] = true;
		}

		return this;
	}

	/// <summary>
	/// Filters the sequence of entities to what does not have the tag.
	/// </summary>
	/// <param name="tag">The tag to filter by.</param>
	/// <returns>The same instance of <see cref="Query{TEntity}"/>.</returns>
	public Query<TEntity> WithoutTag( ReadOnlySpan<char> tag )
	{
		using var enumerator = Entities.GetEnumerator();
		var index = -1;

		while ( enumerator.MoveNext() )
		{
			var entity = enumerator.Current;
			index++;

			if ( IndicesToIgnore[index] )
				continue;

			if ( !entity.HasTag( tag ) )
				continue;

			IndicesToIgnore[index] = true;
		}

		return this;
	}

	/// <summary>
	/// Resets the query's ignored indices for re-filtering.
	/// </summary>
	/// <returns>The same instance of <see cref="Query{TEntity}"/>.</returns>
	public Query<TEntity> Reset()
	{
		for ( var i = 0; i < IndicesToIgnore.Length; i++ )
			IndicesToIgnore[i] = false;

		return this;
	}

	/// <inheritdoc/>
	public IEnumerator<TEntity> GetEnumerator()
	{
		using var enumerator = Entities.GetEnumerator();
		var index = -1;

		while ( enumerator.MoveNext() )
		{
			index++;

			if ( !IndicesToIgnore[index] )
				yield return enumerator.Current;
		}
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator()
	{
		using var enumerator = Entities.GetEnumerator();
		var index = -1;

		while ( enumerator.MoveNext() )
		{
			index++;

			if ( !IndicesToIgnore[index] )
				yield return enumerator.Current;
		}
	}
}

/// <summary>
/// A builder class for creating new queries.
/// </summary>
public static class QueryBuilder
{
	/// <summary>
	/// Creates a new <see cref="Query{TEntity}"/> from a sequence of entities.
	/// </summary>
	/// <typeparam name="T">The type of entities in the sequence.</typeparam>
	/// <param name="entities">The sequence for the query to filter.</param>
	/// <returns>The created <see cref="Query{TEntity}"/>.</returns>
	public static Query<T> From<T>( IEnumerable<T> entities ) where T : IEntity
	{
		return new Query<T>( entities );
	}
}

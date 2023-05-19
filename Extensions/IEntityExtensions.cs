using Sandbox;
using System.Collections.Generic;
using System;

namespace EntityComponentSystem.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="IEntity"/> interface.
/// </summary>
public static class IEntityExtensions
{
	#region Tags
	/// <summary>
	/// Returns whether or not an entity has a tag.
	/// </summary>
	/// <typeparam name="T">The type of the entity to check.</typeparam>
	/// <param name="entity">The entity to check.</param>
	/// <param name="tag">The tag to check for.</param>
	/// <returns>Whether or not an entity has a tag.</returns>
	public static bool HasTag<T>( this T entity, ReadOnlySpan<char> tag ) where T : IEntity
	{
		return HasTag( entity.TagList, tag );
	}

	/// <summary>
	/// Returns a sequence of <see ref="T"/> that contains any of the tags passed.
	/// </summary>
	/// <typeparam name="T">The type of the entities to check.</typeparam>
	/// <param name="entities">The sequence of entities to check.</param>
	/// <param name="tags">An array of all the tags to search for.</param>
	/// <returns>A sequence of <see ref="T"/> that contains any of the tags passed.</returns>
	public static IEnumerable<T> WithAnyTags<T>( this IEnumerable<T> entities, params string[] tags ) where T : IEntity
	{
		foreach ( var entity in entities )
		{
			foreach ( var tag in tags )
			{
				if ( !HasTag( entity.TagList, tag ) )
					continue;

				yield return entity;
				break;
			}
		}
	}

	/// <summary>
	/// Returns a sequence of <see ref="T"/> that contains all of the tags passed.
	/// </summary>
	/// <typeparam name="T">The type of the entities to check.</typeparam>
	/// <param name="entities">The sequence of entities to check.</param>
	/// <param name="tags">An array of all the tags to search for.</param>
	/// <returns>A sequence of <see ref="T"/> that contains all of the tags passed.</returns>
	public static IEnumerable<T> WithTags<T>( this IEnumerable<T> entities, params string[] tags ) where T : IEntity
	{
		foreach ( var entity in entities )
		{
			for ( var i = 0; i < tags.Length; i++ )
			{
				if ( !HasTag( entity.TagList, tags[i] ) )
					break;

				if ( i == tags.Length - 1 )
					yield return entity;
			}
		}
	}

	/// <summary>
	/// Returns a sequence of <see ref="T"/> that does not contain any of the tags passed.
	/// </summary>
	/// <typeparam name="T">The type of the entities to check.</typeparam>
	/// <param name="entities">The sequence of entities to check.</param>
	/// <param name="tags">An array of all the tags to search for.</param>
	/// <returns>A sequence of <see ref="T"/> that does not contain any of the tags passed.</returns>
	public static IEnumerable<T> WithoutTags<T>( this IEnumerable<T> entities, params string[] tags ) where T : IEntity
	{
		foreach ( var entity in entities )
		{
			for ( var i = 0; i < tags.Length; i++ )
			{
				if ( HasTag( entity.TagList, tags[i] ) )
					break;

				if ( i == tags.Length - 1 )
					yield return entity;
			}
		}
	}

	/// <summary>
	/// Returns whether or not a tag is inside a list of tags.
	/// </summary>
	/// <param name="tagList">The list of tags to search.</param>
	/// <param name="tagSpan">The tag to search for.</param>
	/// <returns>Whether or not a tag is inside a list of tags.</returns>
	private static bool HasTag( ReadOnlySpan<char> tagList, ReadOnlySpan<char> tagSpan )
	{
		// Quick check to see if the tag list only contains the tag.
		if ( tagList == tagSpan )
			return true;

		var tagIndex = tagList.IndexOf( tagSpan );
		// Sequence not found.
		if ( tagIndex == -1 )
			return false;

		// Sequence is at the start of the list.
		if ( tagIndex == 0 )
			return tagList[tagIndex + tagSpan.Length] == ' ' ||
				HasTag( tagList[(tagIndex + tagSpan.Length + 1)..], tagSpan );
		// Sequence is at the end of the list.
		else if ( tagIndex == tagList.Length - tagSpan.Length )
			return tagList[tagIndex - 1] == ' ' ||
				HasTag( tagList[..(tagList.Length - tagSpan.Length - 2)], tagSpan );
		// Sequence is somewhere in the middle of the list.
		else
			return (tagList[tagIndex - 1] == ' ' && tagList[tagIndex + tagSpan.Length] == ' ') ||
				HasTag( tagList[(tagIndex + tagSpan.Length + 1)..], tagSpan );
	}
	#endregion

	#region WithComponent
	/// <summary>
	/// Returns a sequence of <see ref="TEntity"/> that contains <see ref="TComponent"/>.
	/// </summary>
	/// <typeparam name="TEntity">The type of entities to check.</typeparam>
	/// <typeparam name="TComponent">The type of the component to check for.</typeparam>
	/// <param name="entities">The sequence of entities to check.</param>
	/// <returns>A sequence of <see ref="TEntity"/> that contains <see ref="TComponent"/>.</returns>
	public static IEnumerable<TEntity> WithComponent<TEntity, TComponent>( this IEnumerable<TEntity> entities )
		where TEntity : IEntity
		where TComponent : IComponent
	{
		foreach ( var entity in entities )
		{
			if ( entity.Components.Count == 0 )
				continue;

			if ( entity.Components.TryGet<TComponent>( out _ ) )
				yield return entity;
		}
	}

	/// <summary>
	/// Returns a sequence of <see ref="TEntity"/> that contains <see ref="TComponent1"/> and <see ref="TComponent2"/>.
	/// </summary>
	/// <typeparam name="TEntity">The type of entities to check.</typeparam>
	/// <typeparam name="TComponent1">The type of the component to check for.</typeparam>
	/// <typeparam name="TComponent2">The type of the component to check for.</typeparam>
	/// <param name="entities">The sequence of entities to check.</param>
	/// <returns>A sequence of <see ref="TEntity"/> that contains <see ref="TComponent1"/> and <see ref="TComponent2"/>.</returns>
	public static IEnumerable<TEntity> WithComponents<TEntity, TComponent1, TComponent2>( this IEnumerable<TEntity> entities )
		where TEntity : IEntity
		where TComponent1 : IComponent
		where TComponent2 : IComponent
	{
		foreach ( var entity in entities )
		{
			if ( entity.Components.Count == 0 )
				continue;

			if ( !entity.Components.TryGet<TComponent1>( out _ ) )
				continue;

			if ( !entity.Components.TryGet<TComponent2>( out _ ) )
				continue;

			yield return entity;
		}
	}

	/// <summary>
	/// Returns a sequence of <see ref="TEntity"/> that contains <see ref="TComponent1"/>, <see ref="TComponent2"/>, and <see ref="TComponent3"/>.
	/// </summary>
	/// <typeparam name="TEntity">The type of entities to check.</typeparam>
	/// <typeparam name="TComponent1">The type of the component to check for.</typeparam>
	/// <typeparam name="TComponent2">The type of the component to check for.</typeparam>
	/// <typeparam name="TComponent3">The type of the component to check for.</typeparam>
	/// <param name="entities">The sequence of entities to check.</param>
	/// <returns>A sequence of <see ref="TEntity"/> that contains <see ref="TComponent1"/>, <see ref="TComponent2"/>, and <see ref="TComponent3"/>.</returns>
	public static IEnumerable<TEntity> WithComponents<TEntity, TComponent1, TComponent2, TComponent3>( this IEnumerable<TEntity> entities )
		where TEntity : IEntity
		where TComponent1 : IComponent
		where TComponent2 : IComponent
		where TComponent3 : IComponent
	{
		foreach ( var entity in entities )
		{
			if ( entity.Components.Count == 0 )
				continue;

			if ( !entity.Components.TryGet<TComponent1>( out _ ) )
				continue;

			if ( !entity.Components.TryGet<TComponent2>( out _ ) )
				continue;

			if ( !entity.Components.TryGet<TComponent3>( out _ ) )
				continue;

			yield return entity;
		}
	}
	#endregion

	#region AsComponent(s)
	/// <summary>
	/// Returns the <see ref="TComponent"/> on an entity.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to get from the entity.</typeparam>
	/// <param name="entity">The entity to get the component from.</param>
	/// <returns>The <see ref="TComponent"/> on an entity.</returns>
	public static TComponent AsComponent<TComponent>( this IEntity entity )
		where TComponent : IComponent
	{
		return entity.Components.Get<TComponent>();
	}

	/// <summary>
	/// Returns the <see ref="TComponent1"/> and <see ref="TComponent2"/> on an entity.
	/// </summary>
	/// <typeparam name="TComponent1">The first type of component to get from the entity.</typeparam>
	/// <typeparam name="TComponent2">The second type of component to get from the entity.</typeparam>
	/// <param name="entity">The entity to get the components from.</param>
	/// <returns>The <see ref="TComponent1"/> and <see ref="TComponent2"/> on an entity.</returns>
	public static (TComponent1, TComponent2) AsComponents<TComponent1, TComponent2>( this IEntity entity )
		where TComponent1 : IComponent
		where TComponent2 : IComponent
	{
		return (entity.Components.Get<TComponent1>(), entity.Components.Get<TComponent2>());
	}

	/// <summary>
	/// Returns the <see ref="TComponent1"/>, <see ref="TComponent2"/>, and <see ref="TComponent3"/> on an entity.
	/// </summary>
	/// <typeparam name="TComponent1">The first type of component to get from the entity.</typeparam>
	/// <typeparam name="TComponent2">The second type of component to get from the entity.</typeparam>
	/// <typeparam name="TComponent3">The third type of component to get from the entity.</typeparam>
	/// <param name="entity">The entity to get the components from.</param>
	/// <returns>The <see ref="TComponent1"/>, <see ref="TComponent2"/>, and <see ref="TComponent3"/> on an entity.</returns>
	public static (TComponent1, TComponent2, TComponent3) AsComponents<TComponent1, TComponent2, TComponent3>( this IEntity entity )
		where TComponent1 : IComponent
		where TComponent2 : IComponent
		where TComponent3 : IComponent
	{
		return (entity.Components.Get<TComponent1>(), entity.Components.Get<TComponent2>(), entity.Components.Get<TComponent3>());
	}
	#endregion
}

using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Queries;

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

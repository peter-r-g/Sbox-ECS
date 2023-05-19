using EntityComponentSystem.Queries;
using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

/// <summary>
/// Defines a system to filter and execute on a sequence of entities.
/// </summary>
/// <typeparam name="TEntity">The type of entities this system works with.</typeparam>
public interface ISystem<TEntity> : IComponent where TEntity : IEntity
{
	/// <summary>
	/// Whether or not the system supports its queries being cached.
	/// </summary>
	bool SupportsCaching { get; }

	/// <summary>
	/// Filters the incoming <see cref="Query{TEntity}"/>.
	/// </summary>
	/// <param name="query">The query to filter.</param>
	void FilterEntities( Query<TEntity> query );
	/// <summary>
	/// Executes the logic of a system on a sequence of entities.
	/// </summary>
	/// <param name="entities">The sequence of entities to execute on.</param>
	/// <param name="args">The arguments passed to the executor.</param>
	void Execute( IEnumerable<TEntity> entities, params object[] args );
}

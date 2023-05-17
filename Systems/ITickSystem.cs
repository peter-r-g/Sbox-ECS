using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

/// <summary>
/// Defines a system that runs on S&amp;boxs tick.
/// </summary>
/// <typeparam name="TEntity">The type of entities this system works with.</typeparam>
public interface ITickSystem<TEntity> : ISystem<TEntity> where TEntity : IEntity
{
	/// <summary>
	/// Invoked on every tick.
	/// </summary>
	/// <param name="entities">The sequence of entities to execute on.</param>
	void Tick( IEnumerable<TEntity> entities );
}

/// <summary>
/// Defines a default implementation of the <see cref="ITickSystem{TEntity}"/> with a constraint of <see cref="IEntity"/>.
/// </summary>
public interface ITickSystem : ITickSystem<IEntity>
{
}

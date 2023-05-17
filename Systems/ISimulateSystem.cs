using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

/// <summary>
/// Defines a system that runs on S&amp;boxs prediction simulation.
/// </summary>
/// <typeparam name="TEntity">The type of entities this system works with.</typeparam>
public interface ISimulateSystem<TEntity> : ISystem<TEntity> where TEntity : IEntity
{
	/// <summary>
	/// Invoked on each input prediction.
	/// </summary>
	/// <param name="entities">The sequence of entities to execute on.</param>
	/// <param name="cl">The client whose input is being predicted.</param>
	void Simulate( IEnumerable<TEntity> entities, IClient cl );
	/// <summary>
	/// Invoked on each frame input prediction.
	/// </summary>
	/// <param name="entities">The sequence of entities to execute on.</param>
	/// <param name="cl">The client whose input is being predicted.</param>
	void FrameSimulate( IEnumerable<TEntity> entities, IClient cl );
}

/// <summary>
/// Defines a default implementation of the <see cref="ISimulateSystem{TEntity}"/> with a constraint of <see cref="IEntity"/>.
/// </summary>
public interface ISimulateSystem : ISimulateSystem<IEntity>
{
}

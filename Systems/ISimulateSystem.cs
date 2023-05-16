using Sandbox;
using System;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

public interface ISimulateSystem<TEntity> : ISystem<TEntity> where TEntity : IEntity
{
	void Simulate( IEnumerable<TEntity> entities, IClient cl );
	void FrameSimulate( IEnumerable<TEntity> entities, IClient cl );
}

public interface ISimulateSystem : ISimulateSystem<IEntity>
{
}

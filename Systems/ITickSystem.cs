using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

public interface ITickSystem<TEntity> : ISystem<TEntity> where TEntity : IEntity
{
	void Tick( IEnumerable<TEntity> entities );
}

public interface ITickSystem : ITickSystem<IEntity>
{
}

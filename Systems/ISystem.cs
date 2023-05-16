using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

public interface ISystem<TEntity> : IComponent where TEntity : IEntity
{
	void FilterEntities( Query<TEntity> query );
	bool Execute( IEnumerable<TEntity> entities, params object[] args );
}

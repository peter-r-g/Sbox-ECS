using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

public interface ISystem : IComponent
{
	void FilterEntities( Query<IEntity> query );
	void Execute( IEnumerable<IEntity> entities, params object[] args );
}

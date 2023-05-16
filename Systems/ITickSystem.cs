using Sandbox;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

public interface ITickSystem : ISystem
{
	void Tick( IEnumerable<IEntity> entities );
}

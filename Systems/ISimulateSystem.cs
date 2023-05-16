using Sandbox;
using System;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

public interface ISimulateSystem : ISystem
{
	void Simulate( IEnumerable<IEntity> entities, IClient cl );
	void FrameSimulate( IEnumerable<IEntity> entities, IClient cl );
}

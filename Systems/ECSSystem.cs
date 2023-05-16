using Sandbox;
using System;

namespace EntityComponentSystem.Systems;

public abstract class ECSSystem : EntityComponent, ISystem
{
	protected override void OnActivate()
	{
		base.OnActivate();

		if ( Entity is not BaseGameManager )
		{
			Entity.Components.Remove( this );
			throw new Exception();
		}
	}
}

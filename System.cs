using Sandbox;
using System;

namespace ECS;

public class System : EntityComponent
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

using Sandbox;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

	public abstract void FilterEntities( Query<IEntity> query );

	public virtual void Execute( IEnumerable<IEntity> entities, params object[] args )
	{
		switch ( this )
		{
			case ISimulateSystem simulateSystem:
				if ( args.Length == 0 || args[0] is not IClient cl )
					throw new InvalidOperationException();

				if ( Game.IsClient && args.Length > 1 && args[1] is bool frameSimulate && frameSimulate )
					simulateSystem.FrameSimulate( entities, cl );
				else
					simulateSystem.Simulate( entities, cl );
				break;
			case ITickSystem tickSystem:
				tickSystem.Tick( entities );
				break;
			default:
				throw new UnreachableException( $"No arm for implementing system. Consider overriding {nameof( ECSSystem )}.{nameof( Execute )}" );
		}
	}
}

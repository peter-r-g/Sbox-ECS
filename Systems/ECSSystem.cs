using Sandbox;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EntityComponentSystem.Systems;

public abstract class ECSSystem<TEntity> : EntityComponent, ISystem<TEntity> where TEntity : IEntity
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

	public abstract void FilterEntities( Query<TEntity> query );

	public virtual bool Execute( IEnumerable<TEntity> entities, params object[] args )
	{
		switch ( this )
		{
			case ISimulateSystem<TEntity> simulateSystem:
				if ( args.Length == 0 || args[0] is not IClient cl )
					throw new InvalidOperationException();

				if ( Game.IsClient && args.Length > 1 && args[1] is bool frameSimulate && frameSimulate )
					simulateSystem.FrameSimulate( entities, cl );
				else
					simulateSystem.Simulate( entities, cl );
				return true;
			case ITickSystem<TEntity> tickSystem:
				tickSystem.Tick( entities );
				return true;
			default:
				return false;
		}
	}
}

public abstract class ECSSystem : ECSSystem<IEntity>
{
}

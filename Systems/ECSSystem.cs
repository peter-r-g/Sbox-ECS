using EntityComponentSystem.Queries;
using Sandbox;
using System;
using System.Collections.Generic;

namespace EntityComponentSystem.Systems;

/// <summary>
/// A default implementation of a system that can be added to the world.
/// </summary>
/// <typeparam name="TEntity">The type of entities this system works with.</typeparam>
public abstract class ECSSystem<TEntity> : EntityComponent, ISystem<TEntity> where TEntity : IEntity
{
	/// <inheritdoc/>
	public abstract bool SupportsCaching { get; }

	/// <inheritdoc/>
	protected override void OnActivate()
	{
		base.OnActivate();

		if ( Entity is not BaseGameManager )
		{
			Entity.Components.Remove( this );
			throw new Exception();
		}
	}

	/// <inheritdoc/>
	public abstract void FilterEntities( Query<TEntity> query );

	/// <inheritdoc/>
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

/// <summary>
/// A default implementation of a system that can be added to the world with the <see cref="IEntity"/> constraint.
/// </summary>
public abstract class ECSSystem : ECSSystem<IEntity>
{
}

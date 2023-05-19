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
	public abstract void Execute( IEnumerable<TEntity> entities, params object[] args );
}

/// <summary>
/// A default implementation of a system that can be added to the world with the <see cref="IEntity"/> constraint.
/// </summary>
public abstract class ECSSystem : ECSSystem<IEntity>
{
}

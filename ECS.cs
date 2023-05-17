using EntityComponentSystem.Systems;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EntityComponentSystem;

/// <summary>
/// A container class for handling all ECS systems. Implemented as a non-static class so extension methods can exist.
/// </summary>
public sealed class ECS
{
	/// <summary>
	/// A reference to the manager of the game that is being played.
	/// </summary>
	private readonly BaseGameManager? gameManager;
	/// <summary>
	/// The configuration applied to the ECS world.
	/// </summary>
	private readonly ECSConfiguration configuration;

	/// <summary>
	/// Initializes a default instance of <see cref="ECS"/>.
	/// </summary>
	/// <exception cref="NotSupportedException">Thrown when an instance of <see cref="ECS"/> already exists.</exception>
	/// <exception cref="InvalidOperationException">Thrown when no <see cref="BaseGameManager"/> was found.</exception>
	public ECS() : this( ECSConfiguration.Default ) { }
	/// <summary>
	/// Initializes an instance of <see cref="ECS"/> with a custom configuration.
	/// </summary>
	/// <param name="configuration">The custom configuration to apply to the ECS world.</param>
	/// <exception cref="InvalidOperationException">Thrown when no <see cref="BaseGameManager"/> was found.</exception>
	public ECS( ECSConfiguration configuration )
	{
		this.configuration = configuration;
		gameManager = Entity.All.OfType<BaseGameManager>().FirstOrDefault();

		if ( gameManager is null )
			throw new InvalidOperationException( "No game manager found, did you create this too early?" );
	}

	/// <summary>
	/// Adds a new system to the world.
	/// </summary>
	/// <typeparam name="TSystem">The type of system to add.</typeparam>
	/// <typeparam name="TEntity">The type of entity that the system works with.</typeparam>
	/// <exception cref="ArgumentException">Thrown when an instance of the system already exists.</exception>
	public void AddSystem<TSystem, TEntity>()
		where TSystem : ISystem<TEntity>, new()
		where TEntity : IEntity
	{
		if ( gameManager!.Components.Get<TSystem>() is not null )
			throw new ArgumentException( $"A system of type {typeof( TSystem ).Name} already exists", nameof( TSystem ) );

		gameManager!.Components.Add( new TSystem() );
	}

	/// <summary>
	/// Adds a new system to the world.
	/// </summary>
	/// <typeparam name="TSystem">The type of system to add.</typeparam>
	public void AddSystem<TSystem>()
		where TSystem : ISystem<IEntity>, new()
	{
		AddSystem<TSystem, IEntity>();
	}

	/// <summary>
	/// Adds an existing system to the world.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity that the system works with.</typeparam>
	/// <param name="system">The system to add.</param>
	public void AddSystem<TEntity>( ISystem<TEntity> system )
		where TEntity : IEntity
	{
		gameManager!.Components.Add( system );
	}

	/// <summary>
	/// Executes systems with a sequence of entities.
	/// </summary>
	/// <typeparam name="TSystem">The type of systems to execute.</typeparam>
	/// <typeparam name="TEntity">The type of entities to execute on.</typeparam>
	/// <param name="entities">The sequence of entities to execute on.</param>
	/// <param name="args">The arguments to pass to the system executor.</param>
	public void Run<TSystem, TEntity>( IEnumerable<TEntity> entities, params object[] args )
		where TSystem : ISystem<TEntity>
		where TEntity : IEntity
	{
		RunInternal<TSystem, TEntity>( entities, args );
	}

	/// <summary>
	/// Executes systems with all entities in S&amp;box that match <see ref="TEntity"/>.
	/// </summary>
	/// <typeparam name="TSystem">The type of systems to execute.</typeparam>
	/// <typeparam name="TEntity">The type of entities to execute on.</typeparam>
	/// <param name="args">The arguments to pass to the system executor.</param>
	public void Run<TSystem, TEntity>( params object[] args )
		where TSystem : ISystem<TEntity>
		where TEntity : IEntity
	{
		RunInternal<TSystem, TEntity>( Entity.All.OfType<TEntity>(), args );
	}

	/// <summary>
	/// Executes systems with all entities in S&amp;box.
	/// </summary>
	/// <typeparam name="TSystem">The type of systems to execute.</typeparam>
	/// <param name="args">The arguments to pass to the system executor.</param>
	public void Run<TSystem>( params object[] args )
		where TSystem : ISystem<IEntity>
	{
		RunInternal<TSystem, IEntity>( Entity.All, args );
	}

	/// The internal method to executing systems on a sequence of entities.
	/// </summary>
	/// <typeparam name="TSystem">The type of systems to execute.</typeparam>
	/// <typeparam name="TEntity">The type of entities to execute on.</typeparam>
	/// <param name="entities">The sequence of entities to execute on.</param>
	/// <param name="args">The arguments to pass to the system executor.</param>
	private void RunInternal<TSystem, TEntity>( IEnumerable<TEntity> entities, object[] args )
		where TSystem : ISystem<TEntity>
		where TEntity : IEntity
	{
		foreach ( var component in gameManager!.Components.GetAll<TSystem>() )
		{
			if ( component is IServerSystem && !Game.IsServer )
				continue;

			if ( component is IClientSystem && !Game.IsClient )
				continue;

			var query = QueryBuilder.From( entities );
			component.FilterEntities( query );

			if ( component.Execute( query.Output, args ) )
				continue;

			if ( configuration.SystemResolver is null || !configuration.SystemResolver( component ) )
				Log.Error( $"{component} failed to find a system method to execute" );
		}
	}
}

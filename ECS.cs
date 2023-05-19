using EntityComponentSystem.Cache;
using EntityComponentSystem.Systems;
using Sandbox;
using Sandbox.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityComponentSystem;

/// <summary>
/// A container class for handling all ECS systems. Implemented as a non-static class so extension methods can exist.
/// </summary>
public sealed class ECS
{
	/// <summary>
	/// The only instance of <see cref="ECS"/> in existence.
	/// </summary>
	public static ECS? Instance { get; private set; }

	/// <summary>
	/// The logger for all ECS related logging.
	/// </summary>
	internal static Logger? Logger { get; private set; }

	/// <summary>
	/// A publically facing cache interface for system queries.
	/// </summary>
	public IQueryCache Cache => cache;
	/// <summary>
	/// A cache for system queries.
	/// </summary>
	private readonly IInternalQueryCache cache;

	/// <summary>
	/// The configuration applied to the ECS world.
	/// </summary>
	internal readonly ECSConfiguration Configuration;

	/// <summary>
	/// A reference to the manager of the game that is being played.
	/// </summary>
	private readonly BaseGameManager? gameManager;

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
		Instance = this;

		// Clone the configuration.
		this.Configuration = new( configuration );
		// FIXME: Libraries don't have base referenced so this is the only way to get the game manager.
		gameManager = Entity.All.OfType<BaseGameManager>().FirstOrDefault();

		if ( gameManager is null )
			throw new InvalidOperationException( "No game manager found, did you create this too early?" );

		// Create cache.
		cache = configuration.UseCaching ? new QueryCache() : new FakeQueryCache();
		// Create logger.
		Logger = new Logger( "ECS" );
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

		if ( Configuration.IsLoggerEnabled( Logs.SystemAdded ) )
			Logger!.Info( $"System \"{typeof( TSystem ).Name}\" added" );
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

		if ( Configuration.IsLoggerEnabled( Logs.SystemAdded ) )
			Logger!.Info( $"System \"{system.GetType().Name}\" added" );
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

	/// <summary>
	/// Executes systems with a sequence of entities.
	/// </summary>
	/// <typeparam name="TEntity">The type of entities to execute on.</typeparam>
	/// <param name="entities">The sequence of entities to execute on.</param>
	/// <param name="args">The arguments to pass to the system executor.</param>
	public void Run<TEntity>( IEnumerable<TEntity> entities, params object[] args )
		where TEntity : IEntity
	{
		RunInternal<ISystem<TEntity>, TEntity>( entities, args );
	}

	/// <summary>
	/// Executes systems with all entities in S&amp;box.
	/// </summary>
	/// <param name="args">The arguments to pass to the system executor.</param>
	public void Run( params object[] args )
	{
		RunInternal<ISystem<IEntity>, IEntity>( Entity.All, args );
	}

	/// <summary>
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

			var query = cache.GetOrCache( component, entities );
			component.Execute( query, args );
		}
	}
}

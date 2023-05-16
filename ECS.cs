using EntityComponentSystem.Systems;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EntityComponentSystem;

public sealed class ECS
{
	private readonly BaseGameManager? gameManager;
	private readonly ECSConfiguration configuration;

	public ECS() : this( ECSConfiguration.Default ) { }
	public ECS( ECSConfiguration configuration )
	{
		this.configuration = configuration;
		gameManager = Entity.All.OfType<BaseGameManager>().FirstOrDefault();

		if ( gameManager is null )
			throw new InvalidOperationException( "No game manager found, did you create this too early?" );
	}

	public void AddSystem<TSystem, TEntity>()
		where TSystem : ISystem<TEntity>, new()
		where TEntity : IEntity
	{
		if ( gameManager!.Components.Get<TSystem>() is not null )
			throw new ArgumentException( $"A system of type {typeof( TSystem ).Name} already exists", nameof( TSystem ) );

		gameManager!.Components.Add( new TSystem() );
	}

	public void AddSystem<TSystem>()
		where TSystem : ISystem<IEntity>, new()
	{
		AddSystem<TSystem, IEntity>();
	}

	public void AddSystem<TEntity>( ISystem<TEntity> system )
		where TEntity : IEntity
	{
		gameManager!.Components.Add( system );
	}

	public void Run<TSystem, TEntity>( IEnumerable<TEntity> entities, params object[] args )
		where TSystem : ISystem<TEntity>
		where TEntity : IEntity
	{
		RunInternal<TSystem, TEntity>( entities, args );
	}

	public void Run<TSystem, TEntity>( params object[] args )
		where TSystem : ISystem<TEntity>
		where TEntity : IEntity
	{
		RunInternal<TSystem, TEntity>( Entity.All.OfType<TEntity>(), args );
	}

	public void Run<TSystem>( params object[] args )
		where TSystem : ISystem<IEntity>
	{
		RunInternal<TSystem, IEntity>( Entity.All, args );
	}

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

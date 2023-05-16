using EntityComponentSystem.Systems;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EntityComponentSystem;

public static class ECS
{
	private static BaseGameManager? gameManager;

	public static void Init()
	{
		gameManager = Entity.All.OfType<BaseGameManager>().First();

		foreach ( var type in TypeLibrary.GetTypes<ECSSystem>() )
		{
			var systemCount = 0;
			foreach ( var @interface in type.Interfaces )
			{
				if ( !@interface.IsAssignableTo( typeof( ISystem ) ) )
					continue;

				if ( @interface == typeof( ISystem ) || @interface == typeof( IClientSystem ) || @interface == typeof( IServerSystem ) )
					continue;

				systemCount++;
			}

			if ( systemCount > 1 )
				Log.Error( $"{type.Name} implements more than one system, this is not supported" );
		}
	}

	public static void AddSystem<T>() where T : ISystem, new()
	{
		AssertGameManager();

		if ( gameManager!.Components.Get<T>() is not null )
			throw new ArgumentException( $"A system of type {typeof( T ).Name} already exists", nameof( T ) );

		gameManager!.Components.Add( new T() );
	}

	public static void AddSystem( ISystem system )
	{
		AssertGameManager();

		gameManager!.Components.Add( system );
	}

	public static void Run<TSystem>( IEnumerable<IEntity> entities, params object[] args ) where TSystem : ISystem
	{
		AssertGameManager();

		RunInternal<TSystem>( entities, args );
	}

	public static void Run<TSystem>( params object[] args ) where TSystem : ISystem
	{
		AssertGameManager();

		RunInternal<TSystem>( Entity.All, args );
	}

	private static void RunInternal<TSystem>( IEnumerable<IEntity> entities, object[] args ) where TSystem : ISystem
	{
		foreach ( var component in gameManager!.Components.GetAll<TSystem>() )
		{
			if ( component is IServerSystem && !Game.IsServer )
				continue;

			if ( component is IClientSystem && !Game.IsClient )
				continue;

			var query = QueryBuilder.From( entities );
			component.FilterEntities( query );

			component.Execute( query.Output, args );
		}
	}

	private static void AssertGameManager( [CallerMemberName] string? method = default )
	{
		method ??= "Unknown";

		if ( gameManager is null )
			throw new InvalidOperationException( $"{method}: {nameof( Init )} must be called before ECS is used" );
	}
}

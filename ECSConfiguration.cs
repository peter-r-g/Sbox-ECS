using Sandbox;
using System;

namespace EntityComponentSystem;

public sealed class ECSConfiguration
{
	public static ECSConfiguration Default => new();

	internal Func<IComponent, bool>? SystemResolver { get; private set; }

	internal ECSConfiguration() { }
	internal ECSConfiguration( ECSConfiguration other )
	{
		SystemResolver = other.SystemResolver;
	}

	public ECSConfiguration WithSystemResolver( Func<IComponent, bool> systemResolver )
	{
		SystemResolver = systemResolver;
		return this;
	}
}

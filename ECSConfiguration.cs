using Sandbox;
using System;

namespace EntityComponentSystem;

/// <summary>
/// Contains configuration options for <see cref="ECS"/>.
/// </summary>
public sealed class ECSConfiguration
{
	/// <summary>
	/// Returns a default instance of <see cref="ECSConfiguration"/>.
	/// </summary>
	public static ECSConfiguration Default => new();

	internal Func<IComponent, bool>? SystemResolver { get; private set; }

	/// <summary>
	/// Initializes a default instance of <see cref="ECSConfiguration"/>.
	/// </summary>
	internal ECSConfiguration() { }
	/// <summary>
	/// Initializes a cloned instance of <see cref="ECSConfiguration"/>.
	/// </summary>
	/// <param name="other"></param>
	internal ECSConfiguration( ECSConfiguration other )
	{
		SystemResolver = other.SystemResolver;
	}

	public ECSConfiguration WithSystemResolver( Func<IComponent, bool> systemResolver )
	/// <summary>
	/// Sets the callback method to resolve custom systems that should be executed.
	/// </summary>
	/// <param name="systemResolver">The callback method to resolve custom systems that should be executed.</param>
	/// <returns>The same instance of <see cref="ECSConfiguration"/>.</returns>
	{
		SystemResolver = systemResolver;
		return this;
	}
}

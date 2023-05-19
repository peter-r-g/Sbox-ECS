using Sandbox;
using System;
using System.Collections;

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

	/// <summary>
	/// A callback method to resolve custom systems that should be executed.
	/// </summary>
	internal Func<IComponent, IEnumerable, object[], bool>? SystemResolver { get; private set; }
	/// <summary>
	/// Whether or not to use query caching.
	/// </summary>
	internal bool UseCaching { get; private set; } = true;
	/// <summary>
	/// Whether or not debug logging is enabled.
	/// </summary>
	internal bool LoggingEnabled { get; private set; } = false;
	/// <summary>
	/// A bit flag containing all of the logs that should be enabled.
	/// </summary>
	internal Logs LogsEnabled { get; private set; } = Logs.All;

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
		UseCaching = other.UseCaching;
	}

	/// <summary>
	/// Sets whether or not to use query caching.
	/// </summary>
	/// <param name="useCaching">Whether or not to use query caching.</param>
	/// <returns>The same instance of <see cref="ECSConfiguration"/>.</returns>
	public ECSConfiguration WithCaching( bool useCaching )
	{
		UseCaching = useCaching;
		return this;
	}

	/// <summary>
	/// Sets the callback method to resolve custom systems that should be executed.
	/// </summary>
	/// <param name="systemResolver">The callback method to resolve custom systems that should be executed.</param>
	/// <returns>The same instance of <see cref="ECSConfiguration"/>.</returns>
	public ECSConfiguration WithSystemResolver( Func<IComponent, IEnumerable, object[], bool> systemResolver )
	{
		SystemResolver = systemResolver;
		return this;
	}

	/// <summary>
	/// Sets whether or not debug logging is enabled.
	/// </summary>
	/// <param name="loggingEnabled">Whether or not logging should be enabled.</param>
	/// <returns>The same instance of <see cref="ECSConfiguration"/>.</returns>
	public ECSConfiguration WithLogging( bool loggingEnabled )
	{
		LoggingEnabled = loggingEnabled;
		return this;
	}

	/// <summary>
	/// Returns whether or not a log is enabled.
	/// </summary>
	/// <param name="log">The log to check.</param>
	/// <returns>Whether or not the log is enabled.</returns>
	internal bool IsLoggerEnabled( Logs log )
	{
		return (LogsEnabled & log) != 0;
	}
}

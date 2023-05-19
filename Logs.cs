using System;

namespace EntityComponentSystem;

/// <summary>
/// A bit flag that defines all logs that exist in ECS.
/// </summary>
[Flags]
public enum Logs
{
	/// <summary>
	/// No logs enabled.
	/// </summary>
	None = 0,
	/// <summary>
	/// When a new system is added.
	/// </summary>
	SystemAdded = 1,
	/// <summary>
	/// When a system is removed.
	/// </summary>
	/// <remarks>This is not yet implemented.</remarks>
	SystemRemoved = 2,
	/// <summary>
	/// When a system has a cached query but it does not have the correct entities contained.
	/// </summary>
	UnexpectedCacheMiss = 4,
	/// <summary>
	/// All logs enabled.
	/// </summary>
	All = int.MaxValue
}

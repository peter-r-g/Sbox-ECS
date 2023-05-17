using Sandbox;

namespace EntityComponentSystem.Cache;

/// <summary>
/// A publically facing interface to a query cache implementation.
/// </summary>
public interface IQueryCache
{
	/// <summary>
	/// Invalidates the entire query cache.
	/// </summary>
	void Invalidate();

	/// <summary>
	/// Invalidates the cache for a specific system.
	/// </summary>
	/// <param name="system">The system whose cache to invalidate.</param>
	void InvalidateFor( IComponent system );
}

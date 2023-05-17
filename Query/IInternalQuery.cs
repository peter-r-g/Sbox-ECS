using System.Collections;

namespace EntityComponentSystem.Queries;

/// <summary>
/// An internal non-generic interface to a <see cref="Query{TEntity}"/>.
/// </summary>
internal interface IInternalQuery
{
	/// <summary>
	/// A non-generic sequence to the entities that are being queried.
	/// </summary>
	IEnumerable Entities { get; }
}

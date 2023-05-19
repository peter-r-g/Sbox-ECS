using Sandbox;

namespace EntityComponentSystem.Components.Transform;

public sealed class PositionComponent : EntityComponent, IPositionComponent
{
	/// <inheritdoc/>
	public Vector3 Position
	{
		get => Entity.Position;
		set => Entity.Position = value;
	}
}

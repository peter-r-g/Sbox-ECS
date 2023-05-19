using Sandbox;

namespace EntityComponentSystem.Components.Transform;

public sealed class RotationComponent : EntityComponent, IRotationComponent
{
	/// <inheritdoc/>
	public Rotation Rotation
	{
		get => Entity.Rotation;
		set => Entity.Rotation = value;
	}
}

using Sandbox;

namespace EntityComponentSystem.Components.Transform;

public sealed class TransformComponent : EntityComponent, IPositionComponent, IRotationComponent, IScaleComponent
{
	/// <inheritdoc/>
	public Vector3 Position
	{
		get => Entity.Position;
		set => Entity.Position = value;
	}

	/// <inheritdoc/>
	public Rotation Rotation
	{
		get => Entity.Rotation;
		set => Entity.Rotation = value;
	}

	/// <inheritdoc/>
	public float Scale
	{
		get => Entity.Scale;
		set => Entity.Scale = value;
	}
}

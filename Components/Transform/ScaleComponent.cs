using Sandbox;

namespace EntityComponentSystem.Components.Transform;

public sealed class ScaleComponent : EntityComponent, IScaleComponent
{
	/// <inheritdoc/>
	public float Scale
	{
		get => Entity.Scale;
		set => Entity.Scale = value;
	}
}

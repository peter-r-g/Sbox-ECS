using Sandbox;

namespace EntityComponentSystem.Components;

public sealed class VelocityComponent : EntityComponent
{
	public Vector3 Velocity
	{
		get => Entity.Velocity;
		set => Entity.Velocity = value;
	}

	public Vector3 LocalVelocity
	{
		get => Entity.LocalVelocity;
		set => Entity.LocalVelocity = value;
	}
	
	public Angles AngularVelocity
	{
		get => Entity.AngularVelocity;
		set => Entity.AngularVelocity = value;
	}

	public Vector3 BaseVelocity
	{
		get => Entity.BaseVelocity;
		set => Entity.BaseVelocity = value;
	}
}

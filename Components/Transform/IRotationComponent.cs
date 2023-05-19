using Sandbox;

namespace EntityComponentSystem.Components.Transform;

public interface IRotationComponent : IComponent
{
	Rotation Rotation { get; set; }
}

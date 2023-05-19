using Sandbox;

namespace EntityComponentSystem.Components.Transform;

public interface IPositionComponent : IComponent
{
	Vector3 Position { get; set; }
}

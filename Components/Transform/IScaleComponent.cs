using Sandbox;

namespace EntityComponentSystem.Components.Transform;

public interface IScaleComponent : IComponent
{
	float Scale { get; set; }
}

using Sandbox;

namespace EntityComponentSystem.Components;

public sealed class ModelComponent : EntityComponent<ModelEntity>
{
	public Model Model
	{
		get => Entity.Model;
		set => Entity.Model = value;
	}

	public string ModelPath
	{
		get => Entity.GetModelName();
		set => Entity.SetModel( value );
	}

	public Color RenderColor
	{
		get => Entity.RenderColor;
		set => Entity.RenderColor = value;
	}
}

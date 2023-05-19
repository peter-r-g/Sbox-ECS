using Sandbox;

namespace EntityComponentSystem.Components;

public sealed class ClientComponent : EntityComponent
{
	public IClient Client => Entity.Client;
}

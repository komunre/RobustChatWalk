using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;

namespace Content.Shared.Items
{
    public class SharedInteractSystem : EntitySystem
    {
        [Dependency] protected readonly IEntityManager _entityManager = default!;
        [Dependency] protected readonly INetManager _netManager = default!;
    }
}
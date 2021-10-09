using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Serialization;
using System;

namespace Content.Shared.Items
{
    public class SharedInteractSystem : EntitySystem
    {
        [Dependency] protected readonly IEntityManager _entityManager = default!;
        [Dependency] protected readonly INetManager _netManager = default!;

    }

    [Serializable, NetSerializable]
    public sealed class InteractUseEvent : EntityEventArgs {
        public EntityUid Sender;
        public EntityUid Target;
        public InteractUseEvent(EntityUid sender, EntityUid target) {
            Sender = sender;
            Target = target;
        }
    }
}
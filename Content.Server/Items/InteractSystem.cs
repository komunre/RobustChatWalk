using Content.Shared.Items;
using Robust.Shared.GameObjects;
using Robust.Shared.Log;

namespace Content.Server.Items
{
    public class InteractSystem : SharedInteractSystem
    {
        public override void Initialize()
        {
            base.Initialize();

            SubscribeNetworkEvent<InteractUseEvent>(OnInteractUse);
        }

        private void OnInteractUse(InteractUseEvent ev, EntitySessionEventArgs args) {
            Logger.Debug("New interact event");
            if (EntityManager.GetEntity(ev.Target).TryGetComponent<ItemComponent>(out var item)) {
                item.Interact(ev.Sender);
            }
        }
    }
}
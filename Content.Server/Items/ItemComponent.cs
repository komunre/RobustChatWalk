using Content.Shared.Items;
using Robust.Shared.GameObjects;
using Content.Server.Inventory;
using Robust.Shared.Log;

namespace Content.Server.Items
{
    [RegisterComponent]
    public class ItemComponent : SharedItemComponent
    {
        public override void Interact(EntityUid id)
        {
            var target = Owner.EntityManager.GetEntity(id);
            if (target.TryGetComponent<InventoryComponent>(out var inventory)) {
                if (!inventory.HasInInventory(Owner)) {
                    inventory.PutIntoInventory(Owner);
                }
            }
        }
    }
}
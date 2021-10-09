using Content.Shared.Items;
using Robust.Shared.GameObjects;
using Content.Server.Inventory;
using Robust.Shared.Log;
using Robust.Shared.IoC;
using Content.Server.Database;

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
                if (Owner.TryGetComponent<EquipmentComponent>(out var equipment))
                {
                    inventory.AddEquipment(equipment);
                    IoCManager.Resolve<ServerDbSqlite>().SaveEquipment(inventory.GetEquipment());
                }
            }
        }
    }
}
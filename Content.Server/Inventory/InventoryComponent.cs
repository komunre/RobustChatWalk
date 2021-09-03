using Robust.Shared.GameObjects;
using Robust.Shared.Maths;
using Content.Shared.Inventory;
using Robust.Shared.IoC;
using System.Collections.Generic;

namespace Content.Server.Inventory
{
    [RegisterComponent]
    public class InventoryComponent : SharedInventoryComponent
    {
        [Dependency] private readonly IEntityManager _entityManager = default!;
        private List<EntityUid> _contained = new();

        public bool HasInInventory(EntityUid id) {
            return _contained.Contains(id);
        }

        public void PutIntoInventory(EntityUid id) {
            _contained.Add(id);
            var ent = _entityManager.GetEntity(id);
            ent.Transform.AttachParent(Owner);
        }
    }
}
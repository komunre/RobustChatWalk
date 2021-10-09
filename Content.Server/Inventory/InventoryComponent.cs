using Robust.Shared.GameObjects;
using Robust.Shared.Maths;
using Content.Shared.Inventory;
using Robust.Shared.IoC;
using System.Collections.Generic;
using Robust.Shared.Containers;
using Content.Shared.Items;

namespace Content.Server.Inventory
{
    [RegisterComponent]
    public class InventoryComponent : SharedInventoryComponent
    {
        //[Dependency] private readonly IEntityManager _entityManager = default!;
        //private List<EntityUid> _contained = new();
        private Container _container = default!;
        private List<EquipmentComponent> _equipment = new();

        protected override void Initialize() {
            base.Initialize();
            
            _container = Owner.EnsureContainer<Container>("inventory");
        }

        public bool HasInInventory(IEntity ent) {
            return _container.Contains(ent);
        }

        public void PutIntoInventory(IEntity ent) {
            _container.Insert(ent);
        }

        public void AddEquipment(EquipmentComponent equipment)
        {
            _equipment.Add(equipment);
        }

        public List<EquipmentComponent> GetEquipment()
        {
            return _equipment;
        }
    }
}
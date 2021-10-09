using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.IoC;
using Robust.Shared.GameObjects;
using Content.Shared.Items;
using Content.Server.Inventory;
using Content.Server.Worlds;
using Content.Shared.GameOjects;
using Robust.Shared.Random;
using Robust.Shared.Log;

namespace Content.Server.Items
{
    class EquipmentSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();

            SubscribeNetworkEvent<EquipmentShootEvent>(HandleShoot);
        }

        private void HandleShoot(EquipmentShootEvent args)
        {
            Logger.Debug("Handling shoot");
            var entity = EntityManager.GetEntity(args.Shooter);
            if (entity.TryGetComponent<InventoryComponent>(out var inv))
            {
                foreach (var eq in inv.GetEquipment())
                {
                    if (eq.Damage > 0)
                    {
                        if (!eq.Ranged)
                        {
                            var target = EntityManager.GetEntity(args.Target);
                            var dist = (entity.Transform.WorldPosition - target.Transform.WorldPosition).Length;
                            if (dist <= 2.0f && target.TryGetComponent<DamageableComponent>(out var damageable))
                            {
                                damageable.Health -= eq.Damage; // ADD DELAY INSIDE OF EQUIPMENT
                                if (!damageable.Owner.HasComponent<ChatterComponent>())
                                {
                                    if (entity.TryGetComponent<ChatterComponent>(out var chatter)) {
                                        chatter.Money += damageable.Reward;
                                    }
                                    damageable.Owner.Delete();
                                }
                            }
                        }
                        // Ranged here
                    }
                }
            }
        }
    }
}

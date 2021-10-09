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
using Robust.Server.Player;
using Robust.Shared.Map;
using Content.Server.Database;

namespace Content.Server.Items
{
    class EquipmentSystem : EntitySystem
    {
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        public override void Initialize()
        {
            base.Initialize();

            SubscribeNetworkEvent<EquipmentShootEvent>(HandleShoot);

            _playerManager.PlayerStatusChanged += AddEquipmentToPlayer;
        }

        private void AddEquipmentToPlayer(object sender, SessionStatusEventArgs e)
        {
            Logger.Debug("trying to add magic wand to player..");
            if (e.NewStatus == Robust.Shared.Enums.SessionStatus.InGame)
            {
                if (e.Session.AttachedEntity != null)
                {
                    if (e.Session.AttachedEntity.TryGetComponent<InventoryComponent>(out var inv)) {
                        var wand = EntityManager.SpawnEntity("magic_wand", new MapCoordinates(new Robust.Shared.Maths.Vector2(0, 0), e.Session.AttachedEntity.Transform.MapID));
                        inv.AddEquipment(wand.GetComponent<EquipmentComponent>());
                        inv.PutIntoInventory(wand);
                        Logger.Debug("Mafic wand added!");
                    }
                }
            }
        }

        private void HandleShoot(EquipmentShootEvent args)
        {
            if (!EntityManager.TryGetEntity(args.Shooter, out var entity)) return;
            if (entity.TryGetComponent<InventoryComponent>(out var inv))
            {
                foreach (var eq in inv.GetEquipment())
                {
                    if (eq.Damage > 0)
                    {
                        if (!eq.Ranged)
                        {
                            if (!EntityManager.TryGetEntity(args.Target, out var target)) return;
                            var dist = (entity.Transform.WorldPosition - target.Transform.WorldPosition).Length;
                            if (dist <= 2.0f && target.TryGetComponent<DamageableComponent>(out var damageable))
                            {
                                damageable.Health -= eq.Damage; // ADD DELAY INSIDE OF EQUIPMENT
                                damageable.Dirty();
                                if (!damageable.Owner.HasComponent<ChatterComponent>())
                                {
                                    if (entity.TryGetComponent<ChatterComponent>(out var chatter)) {
                                        chatter.Money += damageable.Reward;
                                        IoCManager.Resolve<ServerDbSqlite>().SaveWealth(chatter.Owner.Name, chatter.Money);
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

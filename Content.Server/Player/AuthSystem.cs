using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.GameObjects;
using Content.Shared.GameObjects;
using Content.Server.Database;
using Robust.Shared.IoC;
using Content.Shared.GameOjects;
using Robust.Shared.Log;
using Content.Server.Inventory;
using Content.Shared.Items;

namespace Content.Server.Player
{
    class AuthSystem : EntitySystem
    {
        [Dependency] private readonly ServerDbSqlite _db;
        public override void Initialize()
        {
            base.Initialize();

            SubscribeNetworkEvent<AuthRequest>(HandleAuth);
        }

        private void HandleAuth(AuthRequest req)
        {
            Logger.Debug("trying to auth player...");
            var playerEnt = EntityManager.GetEntity(req.RequestUser);
            if (_db.RequirePlayer(playerEnt.Name, req.Password))
            {
                if (playerEnt.TryGetComponent<ChatterComponent>(out var chatter))
                {
                    chatter.Authed = true;
                    chatter.Money = _db.GetWealth(playerEnt.Name);

                    // Add saved equipment
                    if (playerEnt.TryGetComponent<InventoryComponent>(out var inv))
                    {
                        var equipment = _db.GetEquipment(playerEnt.Name);
                        foreach (var eq in equipment)
                        {
                            var eqEnt = EntityManager.SpawnEntity(eq, playerEnt.Transform.Coordinates);
                            if (!eqEnt.TryGetComponent<EquipmentComponent>(out var eqComp)) continue;
                            inv.AddEquipment(eqComp);
                            inv.PutIntoInventory(eqEnt);
                        }
                    }
                    
                    chatter.Dirty();
                    Logger.Info("Player " + playerEnt.Name + " authed");
                }
                return;
            }

            _db.CreatePlayer(playerEnt.Name, req.Password);
            Logger.Info("New player creaated");
        }
    }
}

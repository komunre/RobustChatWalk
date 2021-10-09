using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Content.Shared.Input;
using Robust.Shared.GameObjects;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Players;
using Content.Shared.Items;
using Robust.Shared.IoC;
using Robust.Client.Player;
using Robust.Shared.Log;

namespace Content.Client.Items
{
    class EquipmentSystem : EntitySystem
    {
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        public override void Initialize()
        {
            base.Initialize();

            CommandBinds.Builder
                .Bind(ContentKeyFunctions.Shoot, new PointerInputCmdHandler(ShootHandle))
                .Register<EquipmentSystem>();
            Logger.Debug("Equipment system initialized");
        }

        private bool ShootHandle(in PointerInputCmdHandler.PointerInputCmdArgs args)
        {
            if (args.State != BoundKeyState.Down) return true;

            Logger.Debug("Shooting!");
            var ev = new EquipmentShootEvent();
            ev.Shooter = _playerManager.LocalPlayer.ControlledEntity.Uid;
            ev.Target = args.EntityUid;

            RaiseNetworkEvent(ev);

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.IoC;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Content.Shared.Input;
using Content.Shared.Worlds;
using Robust.Client.Player;
using Robust.Shared.Log;
using Robust.Client.Input;
using Content.Shared.GameOjects;
using Robust.Shared.Players;

namespace Content.Client.Worlds
{
    public class ChangeWorldSystem : EntitySystem
    {
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        public override void Initialize()
        {
            base.Initialize();
            Logger.Debug("Initializing world change system...");
            CommandBinds.Builder
                .Bind(ContentKeyFunctions.WorldJump, InputCmdHandler.FromDelegate(ChangeWorldBind))
                .Register<ChangeWorldSystem>();
        }

        private void ChangeWorldBind(ICommonSession session)
        {
            ChangeWorld();
        }

        private void ChangeWorld()
        {
            Logger.Info("Changing world...");
            var args = new ChangeWorldEvent(_playerManager.LocalPlayer.ControlledEntity.Uid, _playerManager.LocalPlayer.ControlledEntity.Uid, true);
            RaiseNetworkEvent(args);
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);
            if (_playerManager.LocalPlayer.ControlledEntity.HasComponent<ChatterComponent>() && _playerManager.LocalPlayer.ControlledEntity.TryGetComponent<DamageableComponent>(out var damageable))
            {
                if (damageable.Health <= 0)
                {
                    ChangeWorld();
                }
            }
        }
    }
}

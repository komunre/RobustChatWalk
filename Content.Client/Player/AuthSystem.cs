using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Content.Shared.GameObjects;
using Robust.Client.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Client.Player
{
    class AuthSystem : EntitySystem
    {
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        public void SendAuthReq(string pass)
        {
            var request = new AuthRequest();
            request.RequestUser = _playerManager.LocalPlayer.ControlledEntity.Uid;
            request.Password = pass;

            RaiseNetworkEvent(request);
        }
    }
}

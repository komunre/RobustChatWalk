using Content.Shared.Items;
using Robust.Client.Player;
using Robust.Client.Input;
using Robust.Shared.IoC;
using Robust.Client.GameObjects;
using Robust.Shared.Input.Binding;
using Content.Shared.Input;
using Robust.Shared.Players;
using Robust.Shared.Input;

namespace Content.Client.Items
{
    public class InteractSystem : SharedInteractSystem
    {
        [Dependency] private readonly IPlayerManager _playerManager;
        public override void Initialize()
        {
            base.Initialize();

            CommandBinds.Builder
                .Bind(ContentKeyFunctions.Interact, InputCmdHandler.FromDelegate((session) => {
                    
                    var msg = _netManager.CreateNetMessage<InteractMessage>();
                }))
                .Register<InteractSystem>();
        }
    }

    public class MouseClickHandler : InputCmdHandler {
        public override bool HandleCmdMessage(ICommonSession session, InputCmdMessage message)
        {
            return false;
        }
    }
}
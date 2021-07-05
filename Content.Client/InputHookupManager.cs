using Robust.Client.Input;
using Robust.Shared.IoC;
using Robust.Client.Player;
using Robust.Client.GameObjects;
using Robust.Client;
using Robust.Shared.GameObjects;
using Robust.Shared.Input;
using Robust.Shared.Timing;
using Robust.Shared.Map;
using Robust.Shared.Log;

namespace Content.Client
{
    public class InputHookupManager
    {
        [Dependency] private readonly IInputManager _inputManager = default!;
        [Dependency] private readonly IBaseClient _baseClient = default!;
        [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] private readonly IGameTiming _gameTiming = default!;

        public void Initialize() {
            _inputManager.KeyBindStateChanged += OnKeyBindStateChanged;
            Logger.Debug("InputHookupManager initialized");
        }

        public void OnKeyBindStateChanged(ViewportBoundKeyEventArgs args) {
            if (_baseClient.RunLevel != ClientRunLevel.InGame)
                return;

            if (!_entitySystemManager.TryGetEntitySystem<InputSystem>(out var inputSystem)) 
                return;

            var message = new FullInputCmdMessage(_gameTiming.CurTick, _gameTiming.TickFraction, _inputManager.NetworkBindMap.KeyFunctionID(args.KeyEventArgs.Function), args.KeyEventArgs.State, EntityCoordinates.Invalid, args.KeyEventArgs.PointerLocation, EntityUid.Invalid);
            if (inputSystem.HandleInputCommand(_playerManager.LocalPlayer.Session, args.KeyEventArgs.Function, message)) {
                args.KeyEventArgs.Handle();
            }
        }
    }
}
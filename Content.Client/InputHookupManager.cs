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
using Robust.Client.Graphics;

namespace Content.Client
{
    public class InputHookupManager
    {
        [Dependency] private readonly IInputManager _inputManager = default!;
        [Dependency] private readonly IBaseClient _baseClient = default!;
        [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly IEyeManager _eyeManager = default!;

        public void Initialize() {
            _inputManager.KeyBindStateChanged += OnKeyBindStateChanged;
            Logger.Debug("InputHookupManager initialized");
        }

        public void OnKeyBindStateChanged(ViewportBoundKeyEventArgs args) {
            if (_baseClient.RunLevel != ClientRunLevel.InGame)
                return;

            if (!_entitySystemManager.TryGetEntitySystem<InputSystem>(out var inputSystem)) 
                return;

            var plyCoord = _playerManager.LocalPlayer.ControlledEntity.Transform.WorldPosition;
            var coord = _eyeManager.ScreenToMap(args.KeyEventArgs.PointerLocation).Position;
            Logger.Debug("PlayerPos: " + plyCoord);
            Logger.Debug("Point pos: " + coord.X + ":" + coord.Y);
            var entCoord = EntityCoordinates.Invalid;
            var entityLookup = IoCManager.Resolve<IEntityLookup>();
            var entitiesUnder = entityLookup.GetEntitiesInRange(_playerManager.LocalPlayer.ControlledEntity.Transform.MapID, coord, 0.3f);
            IEntity entityUnder = null;
            foreach (var ent in entitiesUnder) {
                entityUnder = ent;
                break;
            }
            EntityUid entityUnderUid;
            if (entityUnder != null) {
                Logger.Debug("Entity detected: " + entityUnder.ToString() + ", " + ((int)entityUnder.Uid));
                entityUnderUid = entityUnder.Uid;
            }
            else {
                entityUnderUid = EntityUid.Invalid;
            }
            var message = new FullInputCmdMessage(_gameTiming.CurTick, _gameTiming.TickFraction, _inputManager.NetworkBindMap.KeyFunctionID(args.KeyEventArgs.Function), args.KeyEventArgs.State, entCoord, args.KeyEventArgs.PointerLocation, entityUnderUid);
            if (inputSystem.HandleInputCommand(_playerManager.LocalPlayer.Session, args.KeyEventArgs.Function, message)) {
                args.KeyEventArgs.Handle();
            }
        }
    }
}
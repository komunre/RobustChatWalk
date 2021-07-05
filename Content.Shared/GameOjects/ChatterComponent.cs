using Robust.Shared.GameObjects;
using Robust.Shared.Maths;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.IoC;
using Robust.Shared.Player;
using Robust.Shared.Players;
using System;
using Robust.Shared.Serialization;
using Robust.Shared.Log;

namespace Content.Shared.GameOjects
{
    [RegisterComponent]
    public class ChatterComponent : Component
    {
        public override string Name => "Chatter";
        public string PlayerName = "default";
        public float Speed = 0.5f;
        public Button PressedButton = Button.None;
    }

    public class ChatterSystem : EntitySystem {
        public override void Initialize()
        {
            base.Initialize();

            CommandBinds.Builder
                .Bind(EngineKeyFunctions.MoveUp, new ButtonInputHandler(Button.Up, SetMovementInput))
                .Bind(EngineKeyFunctions.MoveDown, new ButtonInputHandler(Button.Down, SetMovementInput))
                .Bind(EngineKeyFunctions.MoveRight, new ButtonInputHandler(Button.Right, SetMovementInput))
                .Bind(EngineKeyFunctions.MoveLeft, new ButtonInputHandler(Button.Left, SetMovementInput))
                .Register<ChatterSystem>();
        }

        private void SetMovementInput(ICommonSession session, Button button, bool state) {
            Logger.Debug("Entering button handling...");
            if (session == null || session.AttachedEntity == null || !session.AttachedEntity.TryGetComponent<ChatterComponent>(out var chatter)) {
                return;
            }

            Logger.Debug("Handling button...");

            if (state) {
                chatter.PressedButton = button;
            }
            else {
                chatter.PressedButton = Button.None;
            }

            chatter.Dirty();
        }

        public class ButtonInputHandler  : InputCmdHandler {
            public delegate void MoveHandler(ICommonSession session, Button button, bool state);

            private readonly Button _button;
            private readonly MoveHandler _handler;
            
            public ButtonInputHandler(Button button, MoveHandler handler)
            {
                _button = button;
                _handler = handler;
            }
            
            public override bool HandleCmdMessage(ICommonSession session, InputCmdMessage message)
            {
                Logger.Debug("Pre-handling button...");
                if (message is not FullInputCmdMessage full)
                    return false;

                _handler.Invoke(session, _button, full.State == BoundKeyState.Down);
                return false;
            }
        }
        
    }
    
    [Flags, Serializable, NetSerializable]
    public enum Button {
        None,
        Up,
        Down,
        Right,
        Left,
    }
}
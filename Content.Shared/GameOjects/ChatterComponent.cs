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
using Content.Shared;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.GameOjects
{
    [RegisterComponent, NetworkedComponent]
    public class ChatterComponent : Component
    {
        public override string Name => "Chatter";
        public string PlayerName = "default";
        public float Speed = 0.2f;
        public Button PressedButton = Button.None;
        public int Money = 0;
        
        /*public override ComponentState GetComponentState(ICommonSession session) {
            return new ChatterComponentState(PressedButton, PlayerName);
        }

        public override void HandleComponentState(ComponentState state, ComponentState nextState) {
            if (state is not ChatterComponentState)
                return;
            
            var chState = (ChatterComponentState)state;
            PressedButton = chState.Pressed;
            PlayerName = chState.PlayerName;
        }*/
    }

    public class ChatterSystem : EntitySystem {
        public override void Initialize()
        {
            base.Initialize();

            CommandBinds.Builder
                .Bind(EngineKeyFunctions.MoveUp, new ButtonInputHandler(Button.Up))
                .Bind(EngineKeyFunctions.MoveDown, new ButtonInputHandler(Button.Down))
                .Bind(EngineKeyFunctions.MoveRight, new ButtonInputHandler(Button.Right))
                .Bind(EngineKeyFunctions.MoveLeft, new ButtonInputHandler(Button.Left))
                .Register<ChatterSystem>();

            SubscribeLocalEvent<ChatterComponent, ComponentGetState>(GetChatterState);
            SubscribeLocalEvent<ChatterComponent, ComponentHandleState>(HandleChatterState);
            Logger.Debug("Chatter system initialized");
        }

        private void GetChatterState(EntityUid id, ChatterComponent component, ref ComponentGetState args) {
            args.State = new ChatterComponentState(component.PressedButton, component.PlayerName, component.Money);
        }

        private void HandleChatterState(EntityUid id, ChatterComponent component, ref ComponentHandleState args) {
            if (args.Current is not ChatterComponentState state)
                return;
            
            component.PressedButton = state.Pressed;
            component.PlayerName = state.PlayerName;
        }

        private static void SetMovementInput(ICommonSession session, Button button, bool state) {
            if (session == null || session.AttachedEntity == null || !session.AttachedEntity.TryGetComponent<ChatterComponent>(out var chatter)) {
                return;
            }

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
            //private readonly MoveHandler _handler;
            
            public ButtonInputHandler(Button button)
            {
                _button = button;
                //_handler = handler;
            }

            public override void Enabled(ICommonSession session) {
                //_handler.Invoke(session, _button, true);
            }

            public override void Disabled(ICommonSession session) {
                //_handler.Invoke(session, _button, false);
            }
            
            public override bool HandleCmdMessage(ICommonSession session, InputCmdMessage message)
            {
                if (message is not FullInputCmdMessage full)
                    return false;

                SetMovementInput(session, _button, full.State == BoundKeyState.Down);
                return false;
            }
        }
        
    }

    [Serializable, NetSerializable]
    public class ChatterComponentState : ComponentState {
        public Button Pressed { get; }
        public string PlayerName { get; }
        public int Money = 0;

        public ChatterComponentState(Button pressed, string playerName, int money) : base(/*ContentNetIDs.CHATTER*/) {
            Pressed = pressed;
            PlayerName = playerName;
            Money = money;
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
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Client.State;
using Robust.Shared.GameObjects;
using Robust.Shared.Timing;
using Robust.Client.Player;
using Content.Client.UI;
using Robust.Client.GameObjects;
using Robust.Shared.Log;
using Content.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Client.Placement;
using Robust.Client.ResourceManagement;
using Robust.Shared.Prototypes;

namespace Content.Client
{
    public class GameScreen : State, IEntityEventSubscriber
    {
        [Dependency] protected IEntityManager EntityManager = default!;
        [Dependency] protected IPlayerManager PlayerManager = default!;
        [Dependency] private IEyeManager _eyeManager = default!;
        [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;
        [Dependency] private readonly IInputManager _inputManager = default!;

        public static readonly Vector2i ViewportSize = (EyeManager.PixelsPerMeter * 21, EyeManager.PixelsPerMeter * 15);
        private ChatBox _gameChat;
        public ViewportContainer Viewport { get; private set; }
        private EntitySpawnWindow _spawnEntWindow;
        public override void Startup()
        {
            _gameChat = new ChatBox() { Visible = true };
            var totalData = new TotalData();
            Logger.Debug("GameScreen initialized");

            Viewport = new ViewportContainer {
                AlwaysRender = true,
            };

            _inputManager.SetInputCommand(ContentKeyFunctions.ChatFocus, InputCmdHandler.FromDelegate(_ => {
                _gameChat.ToggleKeyboardFocus();
            }));

            _inputManager.SetInputCommand(ContentKeyFunctions.Send, InputCmdHandler.FromDelegate(_ => {
                // Send message here
            }));

            _spawnEntWindow = new EntitySpawnWindow(IoCManager.Resolve<IPlacementManager>(), IoCManager.Resolve<IPrototypeManager>(), IoCManager.Resolve<IResourceCache>());

            //_spawnEntWindow.Open();
            /*_inputManager.SetInputCommand(ContentKeyFunctions.OpenEntSpawn, InputCmdHandler.FromDelegate(_ => {
                Logger.Debug("Toggling spawn window...");
                if (!_spawnEntWindow.IsOpen) {
                    _spawnEntWindow.Open();
                }
                else {
                    _spawnEntWindow.Close();
                }
            }));*/

            /*CommandBinds.Builder
                .Bind(ContentKeyFunctions.Send, InputCmdHandler.FromDelegate(_ => {
                    _gameChat.SendMessage();
                }))
                .Bind(ContentKeyFunctions.ChatFocus, InputCmdHandler.FromDelegate(_ => {
                    _gameChat.ToggleKeyboardFocus();
                }))
                .Bind(ContentKeyFunctions.OpenEntSpawn,InputCmdHandler.FromDelegate(_ => {
                    Logger.Debug("Toggling spawn window...");
                    if (!_spawnEntWindow.IsOpen) {
                        _spawnEntWindow.Open();
                    }
                    else {
                        _spawnEntWindow.Close();
                    }
                })).Register<GameScreen>();*/

            LayoutContainer.SetAnchorAndMarginPreset(_gameChat, LayoutContainer.LayoutPreset.TopRight);
            LayoutContainer.SetAnchorAndMarginPreset(totalData, LayoutContainer.LayoutPreset.TopLeft);

            //Viewport.AddChild(_gameChat);
            _userInterfaceManager.StateRoot.AddChild(_gameChat);
            _userInterfaceManager.StateRoot.AddChild(totalData);
            //_userInterfaceManager.StateRoot.AddChild(Viewport);

            //_eyeManager.MainViewport = Viewport;
        }

        public override void FrameUpdate(FrameEventArgs e)
        {
            base.FrameUpdate(e);

            var player = PlayerManager.LocalPlayer;
            if (player == null) {
                return;
            }
        }

        public override void Shutdown()
        {
            // Shutdown here
        }
    }
}
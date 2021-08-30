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

namespace Content.Client
{
    public class GameScreen : State, IEntityEventSubscriber
    {
        [Dependency] protected IEntityManager EntityManager = default!;
        [Dependency] protected IPlayerManager PlayerManager = default!;
        [Dependency] private IEyeManager _eyeManager = default!;
        [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;
        
        public static readonly Vector2i ViewportSize = (EyeManager.PixelsPerMeter * 21, EyeManager.PixelsPerMeter * 15);
        private ChatBox _gameChat;
        public ViewportContainer Viewport { get; private set; }
        public override void Startup()
        {
            _gameChat = new ChatBox() { Visible = true };
            Logger.Debug("GameScreen initialized");

            Viewport = new ViewportContainer {
                AlwaysRender = true,
            };

            LayoutContainer.SetAnchorAndMarginPreset(_gameChat, LayoutContainer.LayoutPreset.TopRight);

            //Viewport.AddChild(_gameChat);
            _userInterfaceManager.StateRoot.AddChild(_gameChat);
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
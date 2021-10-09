using Robust.Client.UserInterface;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Maths;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Content.Client.Chat;
using Robust.Client.Player;
using Robust.Shared.GameObjects;
using Content.Client.Player;
using Robust.Shared.Timing;

namespace Content.Client.UI
{
    class AuthBox : Control
    {
        [Dependency] private readonly IEntitySystemManager _systemManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        private LineEdit _passwordInput;
        public AuthBox()
        {
            IoCManager.InjectDependencies(this);
            AddChild(new PanelContainer()
            {
                MinWidth = 300,
                MinHeight = 100,
                Children =
                {
                    new BoxContainer()
                    {
                        Orientation = BoxContainer.LayoutOrientation.Vertical,
                        Children =
                        {
                            new Label()
                            {
                                Text = "Please enter password",
                            },
                            (_passwordInput = new LineEdit()
                            {
                                PlaceHolder = "Password",
                            })
                        }
                    }
                }
            });

            _passwordInput.OnTextEntered += _ =>
            {
                _systemManager.GetEntitySystem<AuthSystem>().SendAuthReq(_passwordInput.Text);
            };
        }
    }
}

using Robust.Client.UserInterface;
using Robust.Client.Graphics;
using Robust.Shared.Physics;
using Robust.Shared.IoC;
using Robust.Shared.GameObjects;
using Content.Shared.GameOjects;
using Robust.Shared.Enums;
using Color = Robust.Shared.Maths.Color;
using Robust.Shared.Prototypes;
using Robust.Shared.Maths;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;
using Robust.Client.Player;

namespace Content.Client.UI
{
    class TotalData : Control
    {
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        private Label _money;
        private Label _health;
        public TotalData()
        {
            IoCManager.InjectDependencies(this);

            AddChild(new PanelContainer()
            {
                MinWidth = 80f,
                MinHeight = 40f,
                Children =
                {
                    new BoxContainer()
                    {
                        Orientation = BoxContainer.LayoutOrientation.Vertical,
                        Children =
                        {
                            (_health = new Label
                            {
                                Text = "Health: 100",
                            }),
                            (_money = new Label
                            {
                                Text = "Money: 0"
                            })
                        }
                    }
                }
            });
        }

        protected override void FrameUpdate(FrameEventArgs args)
        {
            base.FrameUpdate(args);

            if (_playerManager.LocalPlayer == null || _playerManager.LocalPlayer.ControlledEntity == null) return;

            if (_playerManager.LocalPlayer.ControlledEntity.TryGetComponent<ChatterComponent>(out var chatter))
            {
                _money.Text = "Money: " + chatter.Money;
            }

            if (_playerManager.LocalPlayer.ControlledEntity.TryGetComponent<DamageableComponent>(out var damageable))
            {
                _health.Text = "Health: " + damageable.Health;
            }
        }
    }
}

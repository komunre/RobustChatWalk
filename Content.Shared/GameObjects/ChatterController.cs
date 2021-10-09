using Robust.Shared.Players;
using Robust.Shared.Input;
using Robust.Shared.IoC;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.GameObjects;
using Robust.Shared.Maths;
using Robust.Shared.Map;
using Robust.Shared.Timing;

namespace Content.Shared.GameOjects
{
    public class ChatterController : VirtualController
    {
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        public override void UpdateBeforeSolve(bool prediction, float frameTime)
        {
            base.UpdateBeforeSolve(prediction, frameTime);

            foreach (var chatter in EntityManager.EntityQuery<ChatterComponent>()) {
                if (!chatter.Authed) continue;
                var speed = chatter.Speed;
                var direction = Vector2.Zero;

                switch (chatter.PressedButton) { 
                    case Button.Up:
                        direction += Vector2.UnitY;
                        break;
                    case Button.Down:
                        direction -= Vector2.UnitY;
                        break;
                    case Button.Right:
                        direction +=  Vector2.UnitX;
                        break;
                    case Button.Left:
                        direction -= Vector2.UnitX;
                        break;
                }

                chatter.Owner.Transform.WorldPosition += direction * (float)_gameTiming.TickPeriod.TotalSeconds * speed;
            }
        }
    }
}
using Robust.Shared.Players;
using Robust.Shared.Input;
using Robust.Shared.IoC;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.GameObjects;
using Robust.Shared.Maths;
using Robust.Shared.Map;

namespace Content.Shared.GameOjects
{
    public class ChatterController : VirtualController
    {
        public override void UpdateBeforeSolve(bool prediction, float frameTime)
        {
            base.UpdateBeforeSolve(prediction, frameTime);

            foreach (var chatter in EntityManager.EntityQuery<ChatterComponent>()) {
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

                chatter.Owner.Transform.Coordinates += new EntityCoordinates(chatter.Owner.Transform.Parent.Owner.Uid, direction);
            }
        }
    }
}
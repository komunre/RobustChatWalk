using Robust.Client.Graphics;
using Robust.Shared.Physics;
using Robust.Shared.IoC;
using Robust.Shared.GameObjects;
using Content.Shared.GameOjects;
using Robust.Shared.Enums;
using Color = Robust.Shared.Maths.Color;
using Robust.Shared.Prototypes;
using Robust.Shared.Maths;

namespace Content.Client.Overlays
{
    public class ChatterOverlay : Overlay
    {
        [Dependency] private readonly IEntityManager _entityManager = default!;
        [Dependency] private readonly IComponentManager _componentManager = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        private ShaderInstance _shader;

        public override OverlaySpace Space => OverlaySpace.WorldSpace;

        public ChatterOverlay() {
            IoCManager.InjectDependencies(this);
            _shader = _prototypeManager.Index<ShaderPrototype>("unshaded").Instance();
        }
        protected override void Draw(in OverlayDrawArgs args) {
            var handle = args.WorldHandle;

            handle.UseShader(_shader);

            foreach (var chatter in _componentManager.EntityQuery<ChatterComponent>()) {
                handle.DrawCircle(chatter.Owner.Transform.WorldPosition, 1.0f, Color.White);
            }
            handle.DrawRect(new Box2(new Vector2(0, 0), new Vector2(1, 1)), Color.White);

            handle.UseShader(null);
        }
    }
}
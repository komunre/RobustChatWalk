using Robust.Client.Graphics;
using Robust.Shared.IoC;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Enums;
using Robust.Client.GameObjects;

namespace Content.Client.Overlays
{
    public class SpriteOverlay : Overlay
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IEntityManager _entityManager = default!;

        private ShaderInstance _shader;
        public override OverlaySpace Space => OverlaySpace.WorldSpace;
        public SpriteOverlay() {
            IoCManager.InjectDependencies(this);

            _shader = _prototypeManager.Index<ShaderPrototype>("unshaded").Instance();

        }
        protected override void Draw(in OverlayDrawArgs args) {
            var handle = args.WorldHandle;

            handle.UseShader(_shader);

            foreach (var sprite in _entityManager.EntityQuery<SpriteComponent>()) {
                handle.DrawTexture(sprite.Icon.TextureFor(Robust.Shared.Maths.Direction.Invalid), sprite.Owner.Transform.WorldPosition);
            }
            
            handle.UseShader(null);
        }
    }
}
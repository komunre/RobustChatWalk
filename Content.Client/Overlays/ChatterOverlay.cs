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

namespace Content.Client.Overlays
{
    public class ChatterOverlay : Overlay
    {
        [Dependency] private readonly IEntityManager _entityManager = default!;
        [Dependency] private readonly IComponentManager _componentManager = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IResourceCache _resCache = default!;
        private ShaderInstance _shader;
        private TextureResource _playerTexture;
        //private TextureResource _floorTexture;

        public override OverlaySpace Space => OverlaySpace.WorldSpace;

        public ChatterOverlay() {
            IoCManager.InjectDependencies(this);
            _shader = _prototypeManager.Index<ShaderPrototype>("unshaded").Instance();
            _playerTexture = _resCache.GetResource<TextureResource>("/Textures/player.png");
            //_floorTexture = _resCache.GetResource<TextureResource>("/Textures/floor_tile.png");
        }
        protected override void Draw(in OverlayDrawArgs args) {
            var handle = args.WorldHandle;

            handle.UseShader(_shader);

            foreach (var chatter in _componentManager.EntityQuery<ChatterComponent>()) {
                //handle.DrawCircle(new Vector2(chatter.Owner.Transform.MapPosition.X, chatter.Owner.Transform.MapPosition.Y), 1.0f, Color.White);
                //handle.DrawRect(new Box2(new Vector2(0, 0), new Vector2(1, 1)), Color.White, true);
                handle.DrawTexture(_playerTexture.Texture, new Vector2(chatter.Owner.Transform.MapPosition.X, chatter.Owner.Transform.MapPosition.Y));
            }
            //handle.DrawRect(new Box2(new Vector2(0, 0), new Vector2(1, 1)), Color.White);

            handle.UseShader(null);
        }
    }
}
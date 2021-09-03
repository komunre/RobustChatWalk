using Robust.Client.Graphics;
using Robust.Shared.IoC;
using Robust.Shared.GameObjects;
using Robust.Client.ResourceManagement;
using Robust.Client.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Enums;
using Robust.Client.Player;
using Robust.Shared.Maths;

namespace Content.Client.Overlays
{
    public class FloorOverlay : Overlay
    {
        [Dependency] private readonly IResourceCache _resCache = default!;
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        private ShaderInstance _shader;
        public override OverlaySpace Space => OverlaySpace.ScreenSpaceBelowWorld;
        private TextureResource _floorTexture;
        public FloorOverlay() {
            IoCManager.InjectDependencies(this);
            _shader = _prototypeManager.Index<ShaderPrototype>("unshaded").Instance();
            _floorTexture = _resCache.GetResource<TextureResource>("/Textures/floor_tile.png");
        }
        protected override void Draw(in OverlayDrawArgs args) {
            var handle = args.ScreenHandle;
            
            handle.UseShader(_shader);
            
            if (_playerManager?.LocalPlayer?.Session?.AttachedEntity != null) {
                var pos = _playerManager.LocalPlayer.Session.AttachedEntity.Transform.WorldPosition;
                var size = _floorTexture.Texture.Size / (float) EyeManager.PixelsPerMeter;
                for (var x = pos.X - args.WorldBounds.Right; x < args.WorldBounds.Right; x += size.X) {
                    for (var y = pos.Y - args.WorldBounds.Bottom; y < args.WorldBounds.Bottom; y += size.Y) {
                        handle.DrawTexture(_floorTexture.Texture, new Vector2(x, y));
                    }
                }
            }

            handle.UseShader(null);
        }
    }
}
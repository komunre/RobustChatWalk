using Robust.Client.GameObjects;
using Robust.Shared;
using Robust.Shared.Maths;
using Content.Shared;
using Robust.Shared.Map;

namespace Content.Client {
    public class GeneralSystem : SharedGeneralSystem {
        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<PlayerAttachSysMessage>(OnPlayerAttached);
        }

        private void OnPlayerAttached(PlayerAttachSysMessage msg) {
            if (msg.AttachedEntity == null) {
                return;
            }

            var ent = msg.AttachedEntity;
            //var nullEnt = EntityManager.SpawnEntity(null, new MapCoordinates(WalkArenaSize.X / 2f, WalkArenaSize.Y / 2f, ent.Transform.MapID));
            //var eye = nullEnt.AddComponent<EyeComponent>();
            //eye.Current = true;
            //eye.Zoom = Vector2.One;
        }
    }
}
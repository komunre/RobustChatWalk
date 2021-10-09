using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.GameObjects;
using Content.Shared.Worlds;
using Robust.Shared.Map;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Random;

namespace Content.Server.Worlds
{
    public class ChangeWorldSystem : EntitySystem
    {
        [Dependency] private readonly IMapManager _mapManager = default!;
        [Dependency] private readonly IEntitySystemManager _systemManager = default!;
        private List<MapId> _activeMaps = new();
        public override void Initialize()
        {
            SubscribeNetworkEvent<ChangeWorldEvent>(ChangeUserWorld);
        }

        private void ChangeUserWorld(ChangeWorldEvent args)
        {
            var entity = args.Ent;
            var actualEntity = EntityManager.GetEntity(entity);
            var transform = actualEntity.Transform;
            var general = _systemManager.GetEntitySystem<GeneralSystem>();
            if (general.GetMap() != transform.MapID)
            {
                var prevMap = transform.MapID;

                var mapEnt = _mapManager.GetMapEntity(general.GetMap());
                transform.WorldPosition = new Vector2(0, 0);
                transform.AttachParent(mapEnt);

                _mapManager.DeleteMap(prevMap);
                _activeMaps.Remove(prevMap);
                return;
            }
            if (args.NewWorld)
            {
                var newMap = _mapManager.CreateMap();
                var random = IoCManager.Resolve<IRobustRandom>();
                for (int i = 0; i < 10; i++)
                {
                    EntityManager.SpawnEntity("monster", new MapCoordinates(new Vector2(random.Next(-5, 5), random.Next(-5, 5)), newMap));
                }
                _activeMaps.Add(newMap);
                var mapEnt = _mapManager.GetMapEntity(newMap);
                transform.WorldPosition = new Vector2(0, 0);
                transform.AttachParent(mapEnt);
            }
        }
    }
}

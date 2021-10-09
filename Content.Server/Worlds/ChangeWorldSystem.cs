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
using Robust.Shared.Timing;
using Content.Shared.GameOjects;
using Robust.Shared.Log;
using Robust.Shared.Audio;
using Robust.Shared.Player;

namespace Content.Server.Worlds
{
    public class ChangeWorldSystem : EntitySystem
    {
        [Dependency] private readonly IMapManager _mapManager = default!;
        [Dependency] private readonly IEntitySystemManager _systemManager = default!;
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        [Dependency] private readonly IRobustRandom _robustRandom = default!;
        private List<MapId> _activeMaps = new();
        private Dictionary<MapId, float> _difficulties = new();
        private Dictionary<float, string[]> _diffEnemies = new();
        private Dictionary<MapId, float> _spawnTimer = new();
        private float _spawnPeriod = 1;
        private string _music = "/Sound/Music/world_invasion.ogg";
        private Dictionary<MapId, IPlayingAudioStream> _musicStreams = new Dictionary<MapId, IPlayingAudioStream>();
        public override void Initialize()
        {
            SubscribeNetworkEvent<ChangeWorldEvent>(ChangeUserWorld);

            _diffEnemies.Add(0, new[] { "monster" });
            _diffEnemies.Add(3, new[] { "monster", "fasty" });
            _diffEnemies.Add(15, new[] { "tankmer", "fasty", "monster" });
            _diffEnemies.Add(45, new[] { "tankmer", "fasty", "monster", "speedodam" });
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);


            foreach (var world in _activeMaps) {
                _spawnTimer[world] -= (float)_gameTiming.TickPeriod.TotalSeconds;

                var difficulty = _difficulties[world];
                _difficulties[world] += (float)_gameTiming.TickPeriod.TotalSeconds;

                if (_spawnTimer[world] >= 0) continue;
                foreach (var diffGreater in _diffEnemies)
                {
                    if (difficulty > diffGreater.Key)
                    {
                        var enemy_prot = _robustRandom.Pick(diffGreater.Value);
                        EntityManager.SpawnEntity(enemy_prot, new MapCoordinates(_robustRandom.Next(-20, 20), _robustRandom.Next(-20, 20), world));
                       Logger.Debug(enemy_prot + " spawned");
                    }
                }

                _spawnTimer[world] = _spawnPeriod;
            }

            foreach (var chatter in EntityManager.EntityQuery<ChatterComponent>())
            {
                if (!chatter.Owner.TryGetComponent<DamageableComponent>(out var damageable)) continue;
                if (damageable.Health <= 0)
                {
                    ChangeUserWorld(new ChangeWorldEvent(chatter.Owner.Uid, chatter.Owner.Uid, false));
                    damageable.Health = 150;
                }
            }
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
                _difficulties.Remove(prevMap);
                _spawnTimer.Remove(prevMap);
                _musicStreams[prevMap].Stop();
                _musicStreams.Remove(prevMap);
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
                _difficulties.Add(newMap, 0);
                _spawnTimer.Add(newMap, _spawnPeriod);
                var mapEnt = _mapManager.GetMapEntity(newMap);
                transform.WorldPosition = new Vector2(0, 0);
                transform.AttachParent(mapEnt);

                _musicStreams.Add(newMap, SoundSystem.Play(Filter.BroadcastMap(newMap), _music, AudioParams.Default.WithLoop(true)));
            }
        }
    }
}

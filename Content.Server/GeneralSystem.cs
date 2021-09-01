using Robust.Shared.Maths;
using Robust.Shared.GameObjects;
using Robust.Server.GameObjects;
using Content.Shared;
using Robust.Shared.Map;
using Robust.Shared.Random;
using Robust.Shared.IoC;
using Robust.Shared.Player;
using Robust.Server.Player;
using Robust.Shared.Utility;
using Robust.Shared.Enums;
using Robust.Shared.Log;
using Content.Shared.GameOjects;


namespace Content.Server {
    public class GeneralSystem : SharedGeneralSystem { 
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly IMapManager _mapManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        private MapId _map;
        public override void Initialize() {
            base.Initialize();

            _playerManager.PlayerStatusChanged += PlayerStatusChanged;

            StartGame();
        }

        public override void Shutdown() {
            base.Shutdown();
            _mapManager.DeleteMap(_map);
            _playerManager.PlayerStatusChanged -= PlayerStatusChanged;
        }

        private void PlayerStatusChanged(object sender, SessionStatusEventArgs status) {
            if (status.NewStatus == SessionStatus.Connected) {
                status.Session.JoinGame();
            }
            if (status.NewStatus == SessionStatus.InGame) {
                SpawnPlayer(status.Session);
            }
            /*if (status.NewStatus == SessionStatus.Disconnected) {
                //status.Session.AttachedEntity.Delete();
            }*/
        }

        private void SpawnPlayer(IPlayerSession session) {
            Logger.Debug("Spawning player...");
            var random = IoCManager.Resolve<RobustRandom>();
            var entity = EntityManager.SpawnEntity("Chatter",  new MapCoordinates(WalkArenaSize.X / 2f + random.Next(1, 4), WalkArenaSize.Y / 2f + random.Next(1, 4), _map));
            entity.Dirty();
            entity.GetComponent<ChatterComponent>().Dirty();
            session.AttachToEntity(entity);
        }

        private void StartGame() {
            _map = _mapManager.CreateMap();
        }
        public override void Update(float frameTime)
        {
            base.Update(frameTime);
        }
    }
}
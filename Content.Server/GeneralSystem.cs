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

        private void PlayerStatusChanged(object sender, SessionStatusEventArgs status) {
            if (status.NewStatus == SessionStatus.Connected) {
                status.Session.JoinGame();
            }
            if (status.NewStatus == SessionStatus.InGame) {
                SpawnPlayer(status.Session);
            }
        }

        private void SpawnPlayer(IPlayerSession session) {
            Logger.Debug("Spawning player...");
            var entity = EntityManager.SpawnEntity("Chatter",  new MapCoordinates(WalkArenaSize.X / 2f, WalkArenaSize.Y / 2f, _map));
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
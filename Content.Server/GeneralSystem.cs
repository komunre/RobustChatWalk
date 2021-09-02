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
        private GameState _gameState = GameState.Start;
        public override void Initialize() {
            base.Initialize();

            _playerManager.PlayerStatusChanged += PlayerStatusChanged;

            //StartGame();
        }

        public override void Shutdown() {
            base.Shutdown();
            Clear();
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
            //var random = IoCManager.Resolve<RobustRandom>();
            var entity = EntityManager.SpawnEntity("Chatter",  new MapCoordinates(WalkArenaSize.X / 2f, WalkArenaSize.Y / 2f, _map));
            entity.Dirty();
            entity.GetComponent<ChatterComponent>().Dirty();
            session.AttachToEntity(entity);
        }

        private void StartGame() {
            _map = _mapManager.CreateMap();
            var table = EntityManager.SpawnEntity("Table", new MapCoordinates(WalkArenaSize.X / 2f, WalkArenaSize.Y / 2f, _map));
            table.Dirty();
        }

        private void Clear() {
            _mapManager.DeleteMap(_map);
        }
        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            switch (_gameState) {
                case GameState.Start:
                    StartGame();
                    _gameState = GameState.InProgress;
                    break;
                case GameState.End:
                    Clear();
                    _gameState = GameState.Start;
                    break;
            }
        }
    }
}
using Robust.Shared.GameObjects;
using Robust.Shared.Maths;

namespace Content.Shared {
    public enum GameState {
        Start,
        InProgress,
        End,
    }
    public class SharedGeneralSystem : EntitySystem {
        public static readonly Vector2 WalkArenaSize = new Vector2i(20, 10);
        public static readonly Box2 WalkArenaBox = Box2.FromDimensions(Vector2.Zero, WalkArenaSize);
    }
}
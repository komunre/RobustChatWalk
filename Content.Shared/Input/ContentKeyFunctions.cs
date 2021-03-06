using Robust.Shared.Input;

namespace Content.Shared.Input
{
    [KeyFunctions]
    public static class ContentKeyFunctions
    {
        // DEVNOTE: Stick keys you want to be bindable here.
        // public static readonly DummyKey = "DummyKey";
        //public static readonly BoundKeyFunction MoveUp = "MoveUp";
        //public static readonly BoundKeyFunction MoveDown = "MoveDown";
        public static readonly BoundKeyFunction ChatFocus = "ChatFocus";
        public static readonly BoundKeyFunction Send = "Send";
        public static readonly BoundKeyFunction OpenEntSpawn = "OpenEntitySpawn";
        public static readonly BoundKeyFunction Interact = "Interact";
        public static readonly BoundKeyFunction WorldJump = "WorldJump";
        public static readonly BoundKeyFunction Shoot = "Shoot";
    }
}
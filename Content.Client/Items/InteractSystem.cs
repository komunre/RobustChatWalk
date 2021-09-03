using Content.Shared.Items;
using Robust.Shared.Input.Binding;
using Content.Shared.Input;
using Robust.Shared.Input;
using Robust.Shared.Log;
using Robust.Shared.GameObjects;

namespace Content.Client.Items
{
    public class InteractSystem : SharedInteractSystem
    {
        public override void Initialize()
        {
            base.Initialize();

            CommandBinds.Builder
                .Bind(EngineKeyFunctions.Use, new PointerInputCmdHandler(HandleInteract))
                .Register<SharedInteractSystem>();
        }

        private bool HandleInteract(in PointerInputCmdHandler.PointerInputCmdArgs args) {
            if (args.State == BoundKeyState.Down) {
                Logger.Debug("Trying to interact with item...");
                if (args.EntityUid == null || ((int)args.EntityUid) == 0) {
                    Logger.Debug("No entity found");
                    return true;
                }
                var itemUnder = _entityManager.GetEntity(args.EntityUid);
                
                if (itemUnder.TryGetComponent<ItemComponent>(out var interact)) {
                    interact.Interact(args.Session.AttachedEntity.Uid);
                    RaiseNetworkEvent(new InteractUseEvent(args.Session.AttachedEntity.Uid, args.EntityUid));
                    //EntityManager.RaisePredictiveEvent()
                }
            }
            return true;
        }
    }
}
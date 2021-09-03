using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Log;

namespace Content.Shared.Items
{
    //[RegisterComponent]
    public class SharedItemComponent : Component
    {
        //[Dependency] protected readonly IEntityManager _entityManager = default!;
        public override string Name => "Item";

        public virtual void Interact(EntityUid id) { // Virtual is NOT important. This function is NOT important. The only important thing is network events!
            Logger.Debug("Interacting item");
        }

    }
}
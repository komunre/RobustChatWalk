using Content.Shared.Items;

namespace Content.Server.Items
{
    public class InteractSystem : SharedInteractSystem
    {
        
        public override void Initialize() {
            _netManager.RegisterNetMessage<InteractMessage>(OnInteract);
        }

        public void OnInteract(InteractMessage message) {
            
        }
    }
}
using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.GameOjects
{
    [RegisterComponent, NetworkedComponent]
    public class DamageableComponent : Component
    {
        public override string Name => "Damageable";
        [DataField("health", required: true)]
        public float Health;
        [DataField("reward")]
        public int Reward = 0;
    }

    public class DamageableSystem : EntitySystem
    {
        public override void Initialize()
        {
            base.Initialize();

            //SubscribeLocalEvent<DamageableComponent, ComponentGetState>(GetDamageableState);
            //SubscribeLocalEvent<DamageableComponent, ComponentHandleState>(HandleDamageableState);
        }

        private void HandleDamageableState(EntityUid uid, DamageableComponent component, ComponentHandleState args)
        {
            if (args.Current is not DamageableState state) return;
            component.Health = state.Health;
        }

        private void GetDamageableState(EntityUid uid, DamageableComponent component, ComponentGetState args)
        {
            args.State = new DamageableState(component.Health);
        }
    }

    [Serializable, NetSerializable]
    public class DamageableState : ComponentState
    {
        public float Health;
        public DamageableState(float health)
        {
            Health = health;
        }
    }
}

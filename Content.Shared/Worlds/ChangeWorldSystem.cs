using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.Serialization;
using Robust.Shared.GameObjects;

namespace Content.Shared.Worlds
{
    [Serializable, NetSerializable]
    public class ChangeWorldEvent : EntityEventArgs
    {
        public EntityUid Ent;
        public EntityUid Leader;
        public bool NewWorld;
        public ChangeWorldEvent(EntityUid ent, EntityUid leader, bool newWorld)
        {
            Ent = ent;
            Leader = leader;
            NewWorld = newWorld;
        }
    }
}

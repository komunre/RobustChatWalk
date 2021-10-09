using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.Serialization;
using Robust.Shared.GameObjects;

namespace Content.Shared.Items
{
    [Serializable, NetSerializable]
    public class EquipmentShootEvent : EntityEventArgs
    {
        public EntityUid Shooter;
        public EntityUid Target;
    }

    class SharedEquipmentSystem
    {
    }
}

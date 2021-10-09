using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared.Items
{
    [RegisterComponent]
    public class EquipmentComponent : Component
    {
        public override string Name => "Equipment";
        [DataField("damage")]
        public float Damage = 0.0f;
        [DataField("defense")]
        public float Defense = 0.0f;
        [DataField("ranged")]
        public bool Ranged = false;
    }
}

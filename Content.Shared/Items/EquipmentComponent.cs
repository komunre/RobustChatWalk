using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.GameObjects;

namespace Content.Shared.Items
{
    [RegisterComponent]
    public class EquipmentComponent : Component
    {
        public override string Name => "Equipment";
        public float Damage = 0.0f;
        public float Defense = 0.0f;
        public bool Ranged = false;
    }
}

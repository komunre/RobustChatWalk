using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Server.Worlds
{
    [RegisterComponent]
    public class EnemyAIComponent : Component
    {
        public override string Name => "EnemyAI";
        [DataField("speed")]
        public float Speed = 0.23f;
        [DataField("damage")]
        public float Damage = 20f;

        protected override void Initialize()
        {
            base.Initialize();
        }

        
    }
}

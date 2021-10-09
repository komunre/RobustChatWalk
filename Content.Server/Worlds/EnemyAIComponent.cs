using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robust.Shared.GameObjects;

namespace Content.Server.Worlds
{
    [RegisterComponent]
    public class EnemyAIComponent : Component
    {
        public override string Name => "EnemyAI";
        public float Speed = 0.23f;
        public float Damage = 20f;

        protected override void Initialize()
        {
            base.Initialize();
        }

        
    }
}

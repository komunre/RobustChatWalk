using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Content.Shared.GameOjects;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Timing;

namespace Content.Server.Worlds
{
    public class EnemyAISystem : EntitySystem
    {
        [Dependency] private readonly IGameTiming _gameTiming = default!;
        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            foreach (var enemy in EntityManager.EntityQuery<EnemyAIComponent>()) {
                ChatterComponent nearest = null;
                var distance = 9999f;
                foreach (var player in EntityManager.EntityQuery<ChatterComponent>())
                {
                    var dist = player.Owner.Transform.WorldPosition - enemy.Owner.Transform.WorldPosition;
                    if (player.Owner.Transform.MapID == enemy.Owner.Transform.MapID && dist.Length < distance)
                    {
                        distance = dist.Length;
                        nearest = player;
                    }
                }

                if (nearest != null)
                {
                    var dir = nearest.Owner.Transform.WorldPosition - enemy.Owner.Transform.WorldPosition;
                    enemy.Owner.Transform.WorldPosition += dir.Normalized * enemy.Speed * (float)_gameTiming.TickPeriod.TotalSeconds;

                    if (dir.Length < 0.13f && nearest.Owner.TryGetComponent<DamageableComponent>(out var damageable))
                    {
                        damageable.Health -= enemy.Damage  * (float)_gameTiming.TickPeriod.TotalSeconds;
                    }
                }
            }
        }
    }
}

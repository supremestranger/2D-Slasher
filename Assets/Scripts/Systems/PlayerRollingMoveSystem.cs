using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;
using UnityEngine;

namespace Systems
{
    public class PlayerRollingMoveSystem : IEcsRunSystem
    {
        private EcsFilterExt<Player, Rolling> filter;

        public void Run(EcsSystems systems)
        {
            filter.Validate(systems.GetWorld());
            
            foreach (var entity in filter.Filter())
            {
                ref var player = ref filter.Inc1().Get(entity);
                ref var rolling = ref filter.Inc2().Get(entity);

                player.rigidBody.velocity = new Vector2(rolling.rollDir * player.rollSpeed, player.rigidBody.velocity.y);
            }
        }
    }
}
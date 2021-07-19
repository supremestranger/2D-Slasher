using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;
using UnityEngine;

namespace Systems
{
    public class PlayerJumpSystem : IEcsRunSystem
    {
        private EcsFilterExt<Player, Grounded, WantJump, AnimatorRef>.Exc<DoingAttack> filter;
            
        public void Run(EcsSystems systems)
        { 
            filter.Validate(systems.GetWorld());
            
            foreach (var entity in filter.Filter())
            {
                ref var player = ref filter.Inc1().Get(entity);
                ref var animatorRef = ref filter.Inc4().Get(entity);
                player.rigidBody.AddForce(Vector2.up * player.jumpForce);
                animatorRef.animator.SetTrigger("Jump");
            }
        }
    }
}
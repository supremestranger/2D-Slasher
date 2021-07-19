using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;
using UnityEngine;

namespace Systems
{
    public class PlayerStandingMoveSystem : IEcsRunSystem
    {
        private EcsFilterExt<Player, PlayerInput, AnimatorRef>.Exc<Rolling, DoingAttack> filter;
        public void Run(EcsSystems systems)
        {
            filter.Validate(systems.GetWorld());

            foreach (var entity in filter.Filter())
            {
                ref var player = ref filter.Inc1().Get(entity);
                ref var playerInput = ref filter.Inc2().Get(entity);
                ref var animatorRef = ref filter.Inc3().Get(entity);
                
                var rotation = (playerInput.moveInput != 0f)
                    ? playerInput.moveInput
                    : player.playerTranfsorm.localScale.x;
                
                animatorRef.animator.SetFloat("Moving", Mathf.Abs(playerInput.moveInput));
                player.playerTranfsorm.transform.localScale = new Vector3(rotation, 1f, 1f);
                player.rigidBody.velocity = new Vector2(player.speed * playerInput.moveInput, player.rigidBody.velocity.y);
            }
        }
    }
}
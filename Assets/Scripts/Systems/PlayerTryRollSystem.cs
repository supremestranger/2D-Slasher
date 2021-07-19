using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;

namespace Systems
{
    public class PlayerTryRollSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterExt<WantRoll> wantRollFilter;
        private EcsFilterExt<Player, PlayerInput, Grounded, AnimatorRef>.Exc<Rolling> playerGroundedFilter;
        private EcsPool<Rolling> rollingPool;
        private EcsPool<WantRoll> wantRollPool;
        
        public void Init(EcsSystems systems)
        {
            wantRollPool = systems.GetWorld().GetPool<WantRoll>();
            wantRollFilter.Validate(systems.GetWorld());
            playerGroundedFilter.Validate(systems.GetWorld());
            rollingPool = systems.GetWorld().GetPool<Rolling>();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var wantRollEntity in wantRollFilter.Filter())
            {
                wantRollPool.Del(wantRollEntity);
                foreach (var playerEntity in playerGroundedFilter.Filter())
                {
                    ref var player = ref playerGroundedFilter.Inc1().Get(playerEntity);
                    ref var playerInput = ref playerGroundedFilter.Inc2().Get(playerEntity);
                    ref var animatorRef = ref playerGroundedFilter.Inc4().Get(playerEntity);
                    
                    rollingPool.Add(playerEntity);
                    ref var rolling = ref rollingPool.Get(playerEntity);
                    rolling.elapsed = player.rollDuration;
                    rolling.rollDir = playerInput.moveInput;
                    animatorRef.animator.SetTrigger("Roll");
                }
            }
        }
    }
}
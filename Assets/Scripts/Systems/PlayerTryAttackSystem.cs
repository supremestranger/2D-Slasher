using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;

namespace Views
{
    public class PlayerTryAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterExt<Player, PlayerInput, Grounded>.Exc<Rolling, DoingAttack> filter;
        
        private EcsPool<DoAttack> doAttackPool;

        public void Init(EcsSystems systems)
        {
            filter.Validate(systems.GetWorld());
            doAttackPool = systems.GetWorld().GetPool<DoAttack>();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Filter())
            {
                ref var player = ref filter.Inc1().Get(entity);
                ref var playerInput = ref filter.Inc2().Get(entity);

                if (playerInput.attackInput)
                {
                    player.rigidBody.Sleep();
                    doAttackPool.Add(entity);
                }
            }
        }
    }
}
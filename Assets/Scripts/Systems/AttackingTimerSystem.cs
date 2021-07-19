using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;
using UnityEngine;

namespace Views
{
    public class AttackingTimerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterExt<DoingAttack> filter;
        private EcsPool<DoingAttack> attackingPool;
        
        public void Init(EcsSystems systems)
        {
            filter.Validate(systems.GetWorld());
            attackingPool = systems.GetWorld().GetPool<DoingAttack>();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Filter())
            {
                ref var attacking = ref filter.Inc1().Get(entity);

                attacking.elapsed -= Time.deltaTime;

                if (attacking.elapsed <= 0f)
                {
                    attackingPool.Del(entity);
                }
            }
        }
    }
}
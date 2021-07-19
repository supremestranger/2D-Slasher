using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;
using UnityEngine;

namespace Systems
{
    public class PlayerRollingTimerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterExt<Rolling> filter;
        private EcsPool<Rolling> rollingPool;
        
        public void Init(EcsSystems systems)
        {
            filter.Validate(systems.GetWorld());
            rollingPool = systems.GetWorld().GetPool<Rolling>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Filter())
            {
                ref var rolling = ref filter.Inc1().Get(entity);

                rolling.elapsed -= Time.deltaTime;

                if (rolling.elapsed <= 0f)
                {
                    rollingPool.Del(entity);
                }
            }
        }
    }
}
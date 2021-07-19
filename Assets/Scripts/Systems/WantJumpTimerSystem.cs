using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;
using UnityEngine;

namespace Systems
{
    public class WantJumpTimerSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilterExt<WantJump> filter;
        private EcsPool<WantJump> wantJumpPool;

        public void Init(EcsSystems systems)
        {
            filter.Validate(systems.GetWorld());
            wantJumpPool = systems.GetWorld().GetPool<WantJump>();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Filter())
            {
                ref var wantJump = ref filter.Inc1().Get(entity);
                wantJump.timer -= Time.deltaTime;

                if (wantJump.timer <= 0f)
                {
                    wantJumpPool.Del(entity);
                }
            }
        }
    }
}
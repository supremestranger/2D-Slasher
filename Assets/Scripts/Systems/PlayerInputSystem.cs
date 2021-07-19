using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;
using UnityEngine;

namespace Systems
{
    public class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilterExt<PlayerInput> filter;
        private EcsPool<WantJump> wantJumpPool;
        private EcsPool<WantRoll> wantRollPool;
        
        public void Init(EcsSystems systems)
        {
            wantRollPool = systems.GetWorld().GetPool<WantRoll>();
            wantJumpPool = systems.GetWorld().GetPool<WantJump>();
            filter.Validate(systems.GetWorld());
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Filter())
            {
                ref var input = ref filter.Inc1().Get(entity);

                input.moveInput = Input.GetAxisRaw("Horizontal");
                input.attackInput = Input.GetKeyDown(KeyCode.F);

                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    if (!wantRollPool.Has(entity)) wantRollPool.Add(entity);
                }
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!wantJumpPool.Has(entity)) wantJumpPool.Add(entity);
                    ref var wantJump = ref wantJumpPool.Get(entity);
                    wantJump.timer = 0.2f;
                }
            }
        }
    }
}
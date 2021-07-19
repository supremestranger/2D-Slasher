using UnityEngine;
using Components;
using Data;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;

namespace Views
{
    public class EnemySeekPlayerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterExt<Enemy, Seeking, AnimatorRef> filter;
        private EcsPool<Following> followingPool;
        private EcsPool<Seeking> seekingPool;

        public void Init(EcsSystems systems)
        {
            followingPool = systems.GetWorld().GetPool<Following>();
            seekingPool = systems.GetWorld().GetPool<Seeking>();
            filter.Validate(systems.GetWorld());
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Filter())
            {
                var sharedData = systems.GetShared<SharedData>();

                ref var enemy = ref filter.Inc1().Get(entity);

                if (Vector2.Distance(enemy.enemyTransform.position, sharedData.playerTransform.position) <= 5f)
                {
                    ref var animatorRef = ref filter.Inc3().Get(entity);
                    animatorRef.animator.SetBool("Running", true);
                    followingPool.Add(entity);
                    seekingPool.Del(entity);
                }
            }
        }
    }
}
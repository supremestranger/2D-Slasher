using Components;
using Data;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;
using UnityEngine;
using Views;

namespace Systems
{
    public class EnemyFollowingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterExt<Enemy, Following, AnimatorRef>.Exc<DoingAttack> filter;
        private EcsPool<DoAttack> doAttackPool;
        
        public void Init(EcsSystems systems)
        {
            filter.Validate(systems.GetWorld());
            doAttackPool = systems.GetWorld().GetPool<DoAttack>();
        }

        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            foreach (var entity in filter.Filter())
            {
                ref var enemy = ref filter.Inc1().Get(entity);
                ref var animatorRef = ref filter.Inc3().Get(entity);

                var moveDir = sharedData.playerTransform.localPosition.x - enemy.enemyTransform.localPosition.x;
                var rot = 0f;
                if (moveDir >= 1f)
                {
                    rot = -1f;
                }
                else if (moveDir <= -1f)
                {
                    rot = 1f;
                }

                if (rot != 0f)
                    enemy.enemyTransform.localScale = new Vector3(rot, 1f, 1f);
                
                enemy.rigidBody.velocity = new Vector2(moveDir, enemy.rigidBody.velocity.y);
                
                if (Vector2.Distance(enemy.enemyTransform.position, sharedData.playerTransform.position) <= 2f)
                {
                    animatorRef.animator.SetBool("Running", false);
                    doAttackPool.Add(entity);
                }
                else
                {
                    animatorRef.animator.SetBool("Running", true);
                }
            }
        }
    }
}
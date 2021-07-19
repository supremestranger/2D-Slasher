using UnityEngine;
using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;

namespace Views
{
    public class DoMeleeAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterExt<DoAttack, Melee, AnimatorRef> filter;
        private Collider2D[] results = new Collider2D[3];
        private EcsPool<MakeDamage> makeDamagePool;
        private EcsPool<DoAttack> doAttackPool;
        private EcsPool<DoingAttack> attackingPool;

        public void Init(EcsSystems systems)
        {
            makeDamagePool = systems.GetWorld().GetPool<MakeDamage>();
            doAttackPool = systems.GetWorld().GetPool<DoAttack>();
            attackingPool = systems.GetWorld().GetPool<DoingAttack>();
            filter.Validate(systems.GetWorld());
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Filter())
            {
                ref var melee = ref filter.Inc2().Get(entity);
                ref var animatorRef = ref filter.Inc3().Get(entity);

                animatorRef.animator.SetTrigger("Attack");

                attackingPool.Add(entity);
                ref var attacking = ref attackingPool.Get(entity);
                attacking.elapsed = melee.attackDuration;
                var hits = Physics2D.OverlapPointNonAlloc(melee.attackPoint.position, results);

                for (int i = 0; i < hits; i++)
                {
                    if (results[i].TryGetComponent(out EnemyView enemyView))
                    {
                        var unpack = enemyView.entity.Unpack(systems.GetWorld(), out var enemyEntity);
                        if (!unpack) continue;
                        makeDamagePool.Add(enemyEntity);
                        ref var makeDamage = ref makeDamagePool.Get(enemyEntity);
                        makeDamage.value = melee.damage;
                    }
                    
                    if (results[i].TryGetComponent(out PlayerView playerView))
                    {
                        var unpack = playerView.entity.Unpack(systems.GetWorld(), out var playerEntity);
                        if (!unpack) continue;
                        makeDamagePool.Add(playerEntity);
                        ref var makeDamage = ref makeDamagePool.Get(playerEntity);
                        makeDamage.value = melee.damage;
                    }
                }

                doAttackPool.Del(entity);
            }
        }
    }
}
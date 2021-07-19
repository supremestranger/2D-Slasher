using Components;
using Data;
using Leopotam.EcsLite;
using UnityEngine;
using Views;

namespace Systems
{
    public class EnemyInitSystem : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var enemyPool = systems.GetWorld().GetPool<Enemy>();
            var animatorRefPool = systems.GetWorld().GetPool<AnimatorRef>();
            var healthPool = systems.GetWorld().GetPool<Health>();
            var meleePool = systems.GetWorld().GetPool<Melee>();
            var seekingPool = systems.GetWorld().GetPool<Seeking>();
            
            foreach (var enemyView in Object.FindObjectsOfType<EnemyView>())
            {
                var enemyEntity = systems.GetWorld().NewEntity();
                enemyPool.Add(enemyEntity);
                animatorRefPool.Add(enemyEntity);
                healthPool.Add(enemyEntity);
                meleePool.Add(enemyEntity);
                seekingPool.Add(enemyEntity);
                
                ref var enemy = ref enemyPool.Get(enemyEntity);
                ref var animatorRef = ref animatorRefPool.Get(enemyEntity);
                ref var melee = ref meleePool.Get(enemyEntity);
                ref var health = ref healthPool.Get(enemyEntity);

                health.value = enemyView.health;
                animatorRef.animator = enemyView.animator;
                enemy.enemyTransform = enemyView.transform;
                enemy.rigidBody = enemyView.GetComponent<Rigidbody2D>();
                enemyView.entity = systems.GetWorld().PackEntity(enemyEntity);
                melee.damage = enemyView.damage;
                melee.attackDuration = enemyView.attackDuration;
                melee.attackPoint = enemyView.attackPoint;
            }
        }
    }
}
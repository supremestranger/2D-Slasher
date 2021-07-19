using Components;
using Data;
using Leopotam.EcsLite;
using UnityEngine;
using Views;

namespace Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();

            var playerEntity = ecsWorld.NewEntity();

            var playerPool = ecsWorld.GetPool<Player>();
            var playerInputPool = ecsWorld.GetPool<PlayerInput>();
            var animatorRefPoo = ecsWorld.GetPool<AnimatorRef>();
            var meleePool = ecsWorld.GetPool<Melee>();
            var healthPool = ecsWorld.GetPool<Health>();
            playerPool.Add(playerEntity);
            playerInputPool.Add(playerEntity);
            animatorRefPoo.Add(playerEntity);
            meleePool.Add(playerEntity);
            healthPool.Add(playerEntity);
            
            ref var player = ref playerPool.Get(playerEntity);
            ref var animatorRef = ref animatorRefPoo.Get(playerEntity);
            ref var melee = ref meleePool.Get(playerEntity);
            ref var health = ref healthPool.Get(playerEntity);

            health.value = sharedData.staticData.health;
            sharedData.playerTransform = sharedData.sceneData.playerGO.transform;
            sharedData.sceneData.playerGO.GetComponent<PlayerView>().entity = ecsWorld.PackEntity(playerEntity);
            player.rigidBody = sharedData.sceneData.playerGO.GetComponent<Rigidbody2D>();
            player.speed = sharedData.staticData.playerSpeed;
            player.rollSpeed = sharedData.staticData.rollSpeed;
            player.rollDuration = sharedData.staticData.rollDuration;
            player.playerTranfsorm = sharedData.sceneData.playerGO.transform;
            player.jumpForce = sharedData.staticData.jumpForce;
            animatorRef.animator = sharedData.sceneData.playerGO.GetComponent<Animator>();
            melee.damage = sharedData.staticData.damage;
            melee.attackDuration = sharedData.staticData.attackDuration;
            melee.attackPoint = sharedData.sceneData.playerGO.GetComponent<PlayerView>().attackPoint;
        }
    }
}
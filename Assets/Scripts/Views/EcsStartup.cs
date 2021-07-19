using Systems;
using Data;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using UnityEngine;

namespace Views
{
    public sealed class EcsStartup : MonoBehaviour
    {
        private EcsSystems updateSystems;
        private EcsSystems fixedUpdateSystems;
        private EcsWorld ecsWorld;
        public SceneData sceneData;
        public StaticData staticData;

        void Start ()
        {
            ecsWorld = new EcsWorld();
            var sharedData = new SharedData();
            sharedData.sceneData = sceneData;
            sharedData.staticData = staticData;
            EcsPhysicsEvents.ecsWorld = ecsWorld;
            
            updateSystems = new EcsSystems (ecsWorld, sharedData);
            updateSystems
                .Add(new PlayerInitSystem())
                .Add(new EnemyInitSystem())
                .Add(new PlayerInputSystem())
                .Add(new WantJumpTimerSystem())
                .Add(new PlayerTryRollSystem())
                .Add(new PlayerRollingTimerSystem())
                .Add(new PlayerTryAttackSystem())
                .Add(new DoMeleeAttackSystem())
                .Add(new AttackingTimerSystem())
                .Add(new MakeDamageSystem())
                .Add(new EnemySeekPlayerSystem())
                .Add(new EnemyFollowingSystem())
                .Add(new DeathSystem())


#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Init ();

            fixedUpdateSystems = new EcsSystems(ecsWorld, sharedData);
            fixedUpdateSystems
                .Add(new PlayerGroundCheckSystem())
                .Add(new PlayerStandingMoveSystem())
                .Add(new PlayerRollingMoveSystem())
                .Add(new PlayerJumpSystem())
                .DelHerePhysics()
                .Init();
        }
        
        

        void Update () {
            updateSystems?.Run ();
        }

        private void FixedUpdate()
        {
            fixedUpdateSystems?.Run();
        }

        void OnDestroy () {
            if (updateSystems != null) {
                updateSystems.Destroy ();
                ecsWorld.Destroy();
                updateSystems = null;
            }
        }
    }
}
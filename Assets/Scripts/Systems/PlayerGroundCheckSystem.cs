using Components;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;
using Views;

namespace Systems
{
    public class PlayerGroundCheckSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilterExt<OnCollisionEnter2DEvent> enterFilter;
        private EcsFilterExt<OnCollisionExit2DEvent> exitFilter;
        private EcsPool<Grounded> groundedPool;
        private EcsPool<AnimatorRef> animatorPool;
        
        public void Init(EcsSystems systems)
        {
            groundedPool = systems.GetWorld().GetPool<Grounded>();
            animatorPool = systems.GetWorld().GetPool<AnimatorRef>();
            enterFilter.Validate(systems.GetWorld());
            exitFilter.Validate(systems.GetWorld());
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in enterFilter.Filter())
            {
                ref var eventComponent = ref enterFilter.Inc1().Get(entity);
                if (eventComponent.collider2D.gameObject.CompareTag("Ground") &&
                    eventComponent.senderGameObject.CompareTag("GroundCheck"))
                {
                    var packedEntity = eventComponent.senderGameObject.GetComponentInParent<PlayerView>().entity;
                    if (!packedEntity.Unpack(systems.GetWorld(), out var playerEntity)) continue;
                    ref var animator = ref animatorPool.Get(playerEntity);
                    animator.animator.SetBool("InAir", false);
                    if(!groundedPool.Has(playerEntity)) groundedPool.Add(playerEntity);
                }
            }


            foreach (var entity in exitFilter.Filter())
            {
                ref var eventComponent = ref exitFilter.Inc1().Get(entity);
                if (eventComponent.collider2D.gameObject.CompareTag("Ground") &&
                    eventComponent.senderGameObject.CompareTag("GroundCheck"))
                {
                    var packedEntity = eventComponent.senderGameObject.GetComponentInParent<PlayerView>().entity;
                    if (!packedEntity.Unpack(systems.GetWorld(), out var playerEntity)) continue;
                    ref var animator = ref animatorPool.Get(playerEntity);
                    animator.animator.SetBool("InAir", true);
                    if (groundedPool.Has(playerEntity)) groundedPool.Del(playerEntity);
                }
            }
        }
    }
}
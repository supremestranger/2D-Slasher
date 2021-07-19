using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;

namespace Views
{
    public class MakeDamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterExt<MakeDamage, AnimatorRef, Health> filter;
        private EcsPool<MakeDamage> makeDamagePool;
        private EcsPool<Dead> deadPool;

        public void Init(EcsSystems systems)
        {
            filter.Validate(systems.GetWorld());
            makeDamagePool = systems.GetWorld().GetPool<MakeDamage>();
            deadPool = systems.GetWorld().GetPool<Dead>();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Filter())
            {
                ref var makeDamage = ref filter.Inc1().Get(entity);
                ref var animatorRef = ref filter.Inc2().Get(entity);
                ref var health = ref filter.Inc3().Get(entity);
                animatorRef.animator.SetTrigger("Hurt");

                health.value -= makeDamage.value;
                makeDamagePool.Del(entity);

                if (health.value <= 0f)
                {
                    animatorRef.animator.SetTrigger("Death");
                    deadPool.Add(entity);
                }
            }
        }
    }
}
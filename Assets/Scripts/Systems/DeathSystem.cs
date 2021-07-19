using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedFilters;

namespace Views
{
    public class DeathSystem : IEcsRunSystem
    {
        private EcsFilterExt<Dead> filter;
        
        public void Run(EcsSystems systems)
        {
            filter.Validate(systems.GetWorld());
            foreach (var entity in filter.Filter())
            {
                systems.GetWorld().DelEntity(entity);
            }
        }
    }
}
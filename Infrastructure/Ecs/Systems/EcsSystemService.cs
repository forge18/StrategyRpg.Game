using DefaultEcs.System;

namespace Infrastructure.Ecs
{
    public class EcsSystemService : IEcsSystemService
    {
        private ISystem<float> _registeredSystems;

        public void RegisterSystems(ISystem<float> systems)
        {
            _registeredSystems = systems;
        }

        public bool HasSystem()
        {
            return _registeredSystems != null;
        }
    
        public void UpdateSystems(float delta)
        {
            _registeredSystems.Update(delta);
        }
    }
}
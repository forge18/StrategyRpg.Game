using DefaultEcs.System;

namespace Infrastructure.Ecs.Systems
{
    public interface IEcsSystemService
    {
        void RegisterSystems(ISystem<float> systems);
        bool HasSystem();
        void UpdateSystems(float delta);
    }
}
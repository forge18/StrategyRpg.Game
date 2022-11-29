using DefaultEcs.System;

namespace Infrastructure.Ecs
{
    public interface IEcsSystemService
    {
        void RegisterSystems(ISystem<float> systems);
        bool HasSystem();
        void UpdateSystems(float delta);
    }
}
using DefaultEcs.System;

namespace Infrastructure.Ecs
{
    public interface IEcsSystemService
    {
        bool HasSystems();
        SequentialSystem<float> GetRegisteredSystems();
        void ProcessSystems(float delta);
        void LoadUnregisteredSystems();
    }
}
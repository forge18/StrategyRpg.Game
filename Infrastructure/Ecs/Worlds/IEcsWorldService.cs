using DefaultEcs;

namespace Infrastructure.Ecs
{
    public interface IEcsWorldService
    {
        World CreateWorld(EcsWorldEnum worldName);
        World GetWorld(EcsWorldEnum worldName = EcsWorldEnum.Default);
        bool HasWorld(EcsWorldEnum worldName);
        void DestroyWorld(EcsWorldEnum worldName);

        void AddComponentToWorld<T>(T component, EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;
        T GetComponentInWorld<T>(EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;
        T GetComponentReferenceInWorld<T>(EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;
        bool HasComponentInWorld<T>(EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;
        void RemoveComponentFromWorld<T>(EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;

    }
}
using DefaultEcs;

namespace Infrastructure.Ecs.Worlds
{
    public interface IEcsWorldService
    {
        World CreateWorld(string worldName);
        World GetWorld(string worldName = "default");
        bool HasWorld(string worldName);
        void DestroyWorld(string worldName);

        void AddComponentToWorld<T>(T component, string worldName = "default") where T : struct;
        T GetComponentInWorld<T>(string worldName = "default") where T : struct;
        T GetComponentReferenceInWorld<T>(string worldName = "default") where T : struct;
        bool HasComponentInWorld<T>(string worldName = "default") where T : struct;
        void RemoveComponentFromWorld<T>(string worldName = "default") where T : struct;

    }
}
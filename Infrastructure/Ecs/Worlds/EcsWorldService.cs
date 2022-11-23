using System.Collections.Generic;
using DefaultEcs;
using Godot;

namespace Infrastructure.Ecs.Worlds
{
    public class EcsWorldService : IEcsWorldService
    {
        private static Dictionary<string, World> _worlds = new Dictionary<string, World>();

        public World CreateWorld(string worldName)
        {
            if (HasWorld(worldName))
            {
                GD.Print("World already exists");
                return GetWorld(worldName);
            }

            var newWorld = new World();
            _worlds.Add(worldName, newWorld);

            return newWorld;
        }

        public World GetWorld(string worldName = "default")
        {
            if (!HasWorld(worldName))
            {
                GD.Print("World does not exist");
                return default;
            }

            return _worlds[worldName];
        }

        public bool HasWorld(string worldName)
        {
            return _worlds.ContainsKey(worldName);
        }

        public void DestroyWorld(string worldName)
        {
            if (!HasWorld(worldName))
            {
                GD.Print("World does not exist");
                return;
            }

            _worlds[worldName].Dispose();
            _worlds.Remove(worldName);
        }

        public void AddComponentToWorld<T>(T component, string worldName = "default") where T : struct
        {
            var world = GetWorld(worldName);
            world.Set(component);
        }

        public T GetComponentInWorld<T>(string worldName = "default") where T : struct
        {
            var world = GetWorld(worldName);
            var component = world.Get<T>();

            return component;
        }

        public T GetComponentReferenceInWorld<T>(string worldName = "default") where T : struct
        {
            var world = GetWorld(worldName);
            ref var component = ref world.Get<T>();

            return component;
        }

        public bool HasComponentInWorld<T>(string worldName = "default") where T : struct
        {
            var world = GetWorld(worldName);
            var hasComponent = world.Has<T>();

            return hasComponent;
        }

        public void RemoveComponentFromWorld<T>(string worldName = "default") where T : struct
        {
            var world = GetWorld(worldName);
            world.Remove<T>();
        }
    }
}
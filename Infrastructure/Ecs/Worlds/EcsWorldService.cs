using System.Collections.Generic;
using DefaultEcs;
using Godot;

namespace Infrastructure.Ecs
{
    public class EcsWorldService : IEcsWorldService
    {
        private static Dictionary<string, World> _worlds = new Dictionary<string, World>();

        public World CreateWorld(EcsWorldEnum worldName)
        {
            if (HasWorld(worldName))
            {
                GD.Print("World already exists");
                return GetWorld(worldName);
            }

            var newWorld = new World();
            _worlds.Add(worldName.ToString(), newWorld);

            return newWorld;
        }

        public World GetWorld(EcsWorldEnum worldName = EcsWorldEnum.Default)
        {
            if (!HasWorld(worldName))
            {
                CreateWorld(worldName);
            }

            return _worlds[worldName.ToString()];
        }

        public bool HasWorld(EcsWorldEnum worldName)
        {
            return _worlds.ContainsKey(worldName.ToString());
        }

        public void DestroyWorld(EcsWorldEnum worldName)
        {
            if (!HasWorld(worldName))
            {
                GD.Print("World does not exist");
                return;
            }

            _worlds[worldName.ToString()].Dispose();
            _worlds.Remove(worldName.ToString());
        }

        public void AddComponentToWorld<T>(T component, EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct
        {
            var world = GetWorld(worldName);
            world.Set(component);
        }

        public T GetComponentInWorld<T>(EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct
        {
            var world = GetWorld(worldName);
            var component = world.Get<T>();

            return component;
        }

        public T GetComponentReferenceInWorld<T>(EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct
        {
            var world = GetWorld(worldName);
            ref var component = ref world.Get<T>();

            return component;
        }

        public bool HasComponentInWorld<T>(EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct
        {
            var world = GetWorld(worldName);
            var hasComponent = world.Has<T>();

            return hasComponent;
        }

        public void RemoveComponentFromWorld<T>(EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct
        {
            var world = GetWorld(worldName);
            world.Remove<T>();
        }
    }
}
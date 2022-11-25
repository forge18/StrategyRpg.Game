using System;
using System.Collections.Generic;
using Data;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Queries;
using Infrastructure.Ecs.Worlds;

namespace Infrastructure.Ecs.Entities
{
    public class EcsEntityService : IEcsEntityService
    {
        private readonly IEcsWorldService _ecsWorldService;
        private readonly IEcsQueryService _ecsQueryService;
        private readonly IEcsDataLoader _ecsDataLoader;

        public EcsEntityService(IEcsWorldService ecsWorldService, IEcsQueryService ecsQueryService, IEcsDataLoader ecsDataLoader)
        {
            _ecsWorldService = ecsWorldService;
            _ecsQueryService = ecsQueryService;
            _ecsDataLoader = ecsDataLoader;

            TestEntitySetup();
        }

        public void TestEntitySetup()
        {
            var newEntity = _ecsDataLoader.LoadResource(SchemaTypeEnum.Unit, "Godette");
            newEntity.Set<CurrentPosition>(new CurrentPosition { Value = new Vector2(0, 0) });
            newEntity.Set<NeedToRender>();
            newEntity.Set<IsPlayerEntity>();

            GD.Print("tadah!");
        }

        public Entity CreateEntityInWorld(string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entity = world.CreateEntity();

            return entity;
        }

        public Entity GetEntityInWorld(int entityId, string worldName = "default")
        {

            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            return entity;
        }

        public bool HasEntityInWorld(int entityId, string worldName = "default")
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);
            var hasEntity = entity != default;

            return hasEntity;
        }

        public bool HasSchemaIdInWorld(string key, string worldName = "default")
        {
            var entity = _ecsQueryService.GetEntityBySchemaId(key, worldName);
            var hasEntity = entity != default;

            return hasEntity;
        }

        public void DestroyEntityInWorld(int entityId, string worldName = "default")
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            entity.Dispose();
        }

        public List<Entity> ConvertEntityIdsToEntities(int[] entityIds, string worldName = "default")
        {
            var entities = new List<Entity>();

            foreach (var entityId in entityIds)
            {
                var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);
                entities.Add(entity);
            }

            return entities;
        }

        public void AddComponentToEntity<T>(int entityId, T component, string worldName = "default") where T : struct
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            entity.Set(component);
        }

        public T GetComponentInEntity<T>(int entityId, string worldName = "default") where T : struct
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            var component = entity.Get<T>();

            return component;
        }

        public T GetComponentReferenceInEntity<T>(int entityId, string worldName = "default") where T : struct
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            ref var component = ref entity.Get<T>();

            return component;
        }

        public void SetEntityComponent<T>(int entityId, T component, string worldName = "default") where T : struct
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            entity.Set(component);
        }

        public bool HasComponentInEntity<T>(int entityId, string worldName = "default") where T : struct
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            var hasComponent = entity.Has<T>();

            return hasComponent;
        }

        public void RemoveComponentFromEntity<T>(int entityId, string worldName = "default")
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            entity.Remove<T>();
        }

        public void SetEntityEnabled(int entityId, string worldName = "default")
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            entity.Enable();
        }

        public void SetEntityDisabled(int entityId, string worldName = "default")
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            entity.Disable();
        }

        public bool IsEntityEnabled(int entityId, string worldName = "default")
        {
            var entity = _ecsQueryService.GetEntityByEntityId(entityId, worldName);

            return entity.IsEnabled();
        }

        public int ParseEntityId(Entity newEntity)
        {
            var newEntityString = newEntity.ToString();
            int startPoint = newEntityString.IndexOf(":") + ":".Length;
            int endPoint = newEntityString.LastIndexOf(".");

            return Int32.Parse(newEntityString.Substring(startPoint, endPoint - startPoint));
        }
    }
}
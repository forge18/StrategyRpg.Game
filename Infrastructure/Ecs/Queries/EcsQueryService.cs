using System.Collections.Generic;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Worlds;

namespace Infrastructure.Ecs.Queries
{
    public class EcsQueryService : IEcsQueryService
    {
        private readonly IEcsWorldService _ecsWorldService;

        public EcsQueryService(IEcsWorldService ecsWorldService)
        {
            _ecsWorldService = ecsWorldService;
        }

        public Entity GetEntityByEntityId(int entityId, string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entities = world.GetEntities().With<EntityId>().AsSet();
            foreach (var entity in entities.GetEntities())
            {
                var id = entity.Get<EntityId>();
                if (id.Value == entityId)
                {
                    return entity;
                }
            }

            GD.Print("Entity not found");
            return default;
        }

        public Entity GetEntityBySchemaId(string schemaId, string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entities = world.GetEntities().With<SchemaId>().AsSet();
            foreach (var entity in entities.GetEntities())
            {
                var id = entity.Get<SchemaId>();
                if (id.Value == schemaId)
                {
                    return entity;
                }
            }

            GD.Print("Entity not found");
            return default;
        }

        public List<Entity> GetEntitiesBySchemaType(SchemaTypeEnum schemaType, string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entities = world.GetEntities().With<SchemaType>().AsSet();
            var entitiesByType = new List<Entity>();
            foreach (var entity in entities.GetEntities())
            {
                var type = entity.Get<SchemaType>();
                if (type.Value == schemaType)
                {
                    entitiesByType.Add(entity);
                }
            }

            return entitiesByType;
        }

        public Entity GetEntityBySchemaTypeAndSchemaId(SchemaTypeEnum schemaType, string schemaId, string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entities = world.GetEntities().With<SchemaType>().With<SchemaId>().AsSet();
            foreach (var entity in entities.GetEntities())
            {
                var type = entity.Get<SchemaType>();
                var id = entity.Get<SchemaId>();
                if (type.Value == schemaType && id.Value == schemaId)
                {
                    return entity;
                }
            }

            GD.Print("Entity not found");
            return default;
        }

        public List<Entity> GetEntitiesWith<T>(string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entities = world.GetEntities().With<T>().AsSet();
            var entityList = new List<Entity>();
            foreach (var entity in entities.GetEntities())
            {
                entityList.Add(entity);
            }

            return entityList;
        }

        public List<Entity> GetEntitiesWith<T1, T2>(string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entities = world.GetEntities().With<T1>().With<T2>().AsSet();
            var entityList = new List<Entity>();
            foreach (var entity in entities.GetEntities())
            {
                entityList.Add(entity);
            }

            return entityList;
        }

        public List<Entity> GetEntitiesWhere<T>(int value, string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entities = world.GetEntities().With<T>().AsSet();
            var entityList = new List<Entity>();
            foreach (var entity in entities.GetEntities())
            {
                var component = entity.Get<T>();
                if (component.Equals(value))
                {
                    entityList.Add(entity);
                }
            }

            return entityList;
        }

        public List<Entity> GetEntitiesWhere<T>(string value, string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entities = world.GetEntities().With<T>().AsSet();
            var entityList = new List<Entity>();
            foreach (var entity in entities.GetEntities())
            {
                var component = entity.Get<T>();
                if (component.Equals(value))
                {
                    entityList.Add(entity);
                }
            }

            return entityList;
        }

        public List<Entity> GetEntitiesWhere<T>(Vector2 value, string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entities = world.GetEntities().With<T>().AsSet();
            var entityList = new List<Entity>();
            foreach (var entity in entities.GetEntities())
            {
                var component = entity.Get<T>();
                if (component.Equals(value))
                {
                    entityList.Add(entity);
                }
            }

            return entityList;
        }
    }
}
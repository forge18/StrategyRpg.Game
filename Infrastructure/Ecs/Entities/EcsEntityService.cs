using System;
using DefaultEcs;
using Infrastructure.Ecs.Components;

namespace Infrastructure.Ecs
{
    public class EcsEntityService : IEcsEntityService
    {
        private readonly World _world;

        public EcsEntityService(IEcsWorldService ecsWorldService)
        {
            _world = ecsWorldService.GetWorld();
        }

        public Entity CreateEntityInWorld(EcsWorldEnum worldName = EcsWorldEnum.Default)
        {
            var entity = _world.CreateEntity();
            entity.Set<EntityId>(new EntityId { Value = ParseEntityId(entity) });

            return entity;
        }

        public bool HasSchemaIdInWorld(string key, EcsWorldEnum worldName = EcsWorldEnum.Default)
        {
            var entity = GetEntityBySchemaId(key);
            var hasEntity = entity != default;

            return hasEntity;
        }

        public void DestroyEntityInWorld(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default)
        {
            var entity = GetEntityByEntityId(entityId);

            entity.Dispose();
        }

        public void SetEntityEnabled(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default)
        {
            var entity = GetEntityByEntityId(entityId);

            entity.Enable();
        }

        public void SetEntityDisabled(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default)
        {
            var entity = GetEntityByEntityId(entityId);

            entity.Disable();
        }

        public bool IsEntityEnabled(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default)
        {
            var entity = GetEntityByEntityId(entityId);

            return entity.IsEnabled();
        }

        public int ParseEntityId(Entity newEntity)
        {
            var newEntityString = newEntity.ToString();
            int startPoint = newEntityString.IndexOf(":") + ":".Length;
            int endPoint = newEntityString.LastIndexOf(".");

            return Int32.Parse(newEntityString.Substring(startPoint, endPoint - startPoint));
        }

        private Entity GetEntityByEntityId(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default)
        {
            var entities = _world.GetEntities().With<EntityId>().AsSet().GetEntities();

            foreach (var entity in entities)
            {
                var eId = entity.Get<EntityId>();
                if (eId.Value == entityId)
                {
                    return entity;
                }
            }

            return default;
        }

        private Entity GetEntityBySchemaId(string schemaId, EcsWorldEnum worldName = EcsWorldEnum.Default)
        {
            var entities = _world.GetEntities().With<SchemaId>().AsSet().GetEntities();

            foreach (var entity in entities)
            {
                var eSchemaId = entity.Get<SchemaId>();
                if (eSchemaId.Value == schemaId)
                {
                    return entity;
                }
            }

            return default;
        }
    }
}
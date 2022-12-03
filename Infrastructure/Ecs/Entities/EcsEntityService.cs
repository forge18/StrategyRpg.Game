using System;
using System.Collections.Generic;
using DefaultEcs;
using Features.Global;
using Infrastructure.HubMediator;
using Infrastructure.Ecs.Components;

namespace Infrastructure.Ecs
{
    public class EcsEntityService : IEcsEntityService
    {
        private readonly IMediator _mediator;
        private readonly IEcsWorldService _ecsWorldService;

        public EcsEntityService(IMediator mediator, IEcsWorldService ecsWorldService)
        {
            _mediator = mediator;
            _ecsWorldService = ecsWorldService;
        }

        public Entity CreateEntityInWorld(string worldName = "default")
        {
            var world = _ecsWorldService.GetWorld(worldName);
            var entity = world.CreateEntity();
            entity.Set<EntityId>(new EntityId { Value = ParseEntityId(entity) });

            return entity;
        }

        public Entity GetEntityInWorld(int entityId, string worldName = "default")
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            return entity;
        }

        public bool HasEntityInWorld(int entityId, string worldName = "default")
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();
            var hasEntity = entity != default;

            return hasEntity;
        }

        public bool HasSchemaIdInWorld(string key, string worldName = "default")
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityBySchemaId,
                new GetEntityBySchemaIdQuery { SchemaId = key }
            );
            var entity = result.ConvertResultValue<Entity>();
            var hasEntity = entity != default;

            return hasEntity;
        }

        public void DestroyEntityInWorld(int entityId, string worldName = "default")
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            entity.Dispose();
        }

        public List<Entity> ConvertEntityIdsToEntities(int[] entityIds, string worldName = "default")
        {
            var entities = new List<Entity>();

            foreach (var entityId in entityIds)
            {
                var result = _mediator.RunQuery(
                    QueryTypeEnum.GetEntityByEntityId,
                    new GetEntityByEntityIdQuery(worldName, entityId)
                );
                var entity = result.ConvertResultValue<Entity>();
                entities.Add(entity);
            }

            return entities;
        }

        public void AddComponentToEntity<T>(int entityId, T component, string worldName = "default") where T : struct
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            entity.Set(component);
        }

        public T GetComponentInEntity<T>(int entityId, string worldName = "default") where T : struct
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            var component = entity.Get<T>();

            return component;
        }

        public T GetComponentReferenceInEntity<T>(int entityId, string worldName = "default") where T : struct
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            ref var component = ref entity.Get<T>();

            return component;
        }

        public void SetEntityComponent<T>(int entityId, T component, string worldName = "default") where T : struct
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            entity.Set(component);
        }

        public bool HasComponentInEntity<T>(int entityId, string worldName = "default") where T : struct
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            var hasComponent = entity.Has<T>();

            return hasComponent;
        }

        public void RemoveComponentFromEntity<T>(int entityId, string worldName = "default")
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            entity.Remove<T>();
        }

        public void SetEntityEnabled(int entityId, string worldName = "default")
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            entity.Enable();
        }

        public void SetEntityDisabled(int entityId, string worldName = "default")
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

            entity.Disable();
        }

        public bool IsEntityEnabled(int entityId, string worldName = "default")
        {
            var result = _mediator.RunQuery(
                QueryTypeEnum.GetEntityByEntityId,
                new GetEntityByEntityIdQuery(worldName, entityId)
            );
            var entity = result.ConvertResultValue<Entity>();

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
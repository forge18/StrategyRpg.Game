using System.Collections.Generic;
using DefaultEcs;

namespace Infrastructure.Ecs
{
    public interface IEcsEntityService
    {
        Entity CreateEntityInWorld(EcsWorldEnum worldName = EcsWorldEnum.Default);
        Entity GetEntityInWorld(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        bool HasEntityInWorld(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        bool HasSchemaIdInWorld(string schemaId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        void DestroyEntityInWorld(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        List<Entity> ConvertEntityIdsToEntities(int[] entityIds, EcsWorldEnum worldName = EcsWorldEnum.Default);

        void AddComponentToEntity<T>(int entityId, T component, EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;
        T GetComponentInEntity<T>(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;
        T GetComponentReferenceInEntity<T>(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;
        void SetEntityComponent<T>(int entityId, T component, EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;
        bool HasComponentInEntity<T>(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default) where T : struct;
        void RemoveComponentFromEntity<T>(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);

        void SetEntityEnabled(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        void SetEntityDisabled(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        bool IsEntityEnabled(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        int ParseEntityId(Entity newEntity);
    }
}
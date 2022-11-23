using System.Collections.Generic;
using DefaultEcs;

namespace Infrastructure.Ecs.Entities
{
    public interface IEcsEntityService
    {
        Entity CreateEntityInWorld(string worldName = "default");
        Entity GetEntityInWorld(int entityId, string worldName = "default");
        bool HasEntityInWorld(int entityId, string worldName = "default");
        bool HasSchemaIdInWorld(string schemaId, string worldName = "default");
        void DestroyEntityInWorld(int entityId, string worldName = "default");
        List<Entity> ConvertEntityIdsToEntities(int[] entityIds, string worldName = "default");

        void AddComponentToEntity<T>(int entityId, T component, string worldName = "default") where T : struct;
        T GetComponentInEntity<T>(int entityId, string worldName = "default") where T : struct;
        T GetComponentReferenceInEntity<T>(int entityId, string worldName = "default") where T : struct;
        void SetEntityComponent<T>(int entityId, T component, string worldName = "default") where T : struct;
        bool HasComponentInEntity<T>(int entityId, string worldName = "default") where T : struct;
        void RemoveComponentFromEntity<T>(int entityId, string worldName = "default");

        void SetEntityEnabled(int entityId, string worldName = "default");
        void SetEntityDisabled(int entityId, string worldName = "default");
        bool IsEntityEnabled(int entityId, string worldName = "default");
        int ParseEntityId(Entity newEntity);
    }
}
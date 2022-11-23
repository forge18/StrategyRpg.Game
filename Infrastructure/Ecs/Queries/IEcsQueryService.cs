using System.Collections.Generic;
using DefaultEcs;
using Godot;

namespace Infrastructure.Ecs.Queries
{
    public interface IEcsQueryService
    {
        Entity GetEntityByEntityId(int entityId, string worldName = "default");
        Entity GetEntityBySchemaId(string schemaId, string worldName = "default");
        List<Entity> GetEntitiesBySchemaType(SchemaTypeEnum schemaType, string worldName = "default");
        Entity GetEntityBySchemaTypeAndSchemaId(SchemaTypeEnum schemaType, string schemaId, string worldName = "default");

        List<Entity> GetEntitiesWith<T>(string worldName = "default");
        List<Entity> GetEntitiesWith<T1, T2>(string worldName = "default");
        List<Entity> GetEntitiesWhere<T>(int value, string worldName = "default");
        List<Entity> GetEntitiesWhere<T>(string value, string worldName = "default");
        List<Entity> GetEntitiesWhere<T>(Vector2 value, string worldName = "default");
    }
}
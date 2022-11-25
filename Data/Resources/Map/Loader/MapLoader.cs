using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.Map.Loader
{
    public class MapLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public MapLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public Entity MapDataToEntity(MapSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.Name == null) return default;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return default;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsMap>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.Map });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.Name });

            if (schema.Description != null)
                newEntity.Set<Description>(new Description() { Value = schema.Description });

            if (schema.ScenePath != null)
                newEntity.Set<MapScenePath>(new MapScenePath() { Value = schema.ScenePath });

            return newEntity;
        }
    }
}

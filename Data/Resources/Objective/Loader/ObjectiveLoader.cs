using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.Objective.Loader
{
    public class ObjectiveLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public ObjectiveLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public Entity MapDataToEntity(ObjectiveSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.Name == null) return default;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return default;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsObjective>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.Objective });
            newEntity.Set<Name>(new Name() { Value = schema.Name });

            if (schema.Description != null)
                newEntity.Set<Description>(new Description() { Value = schema.Description });

            return newEntity;
        }
    }
}

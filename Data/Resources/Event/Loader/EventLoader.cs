using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.Ability.Loader
{
    public class EventLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public EventLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public void MapDataToEntity(EventSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.Name == null) return;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsEvent>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.ABILITY });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.Name });

            if (schema.Description != null)
                newEntity.Set<Description>(new Description() { Value = schema.Description });
        }
    }
}

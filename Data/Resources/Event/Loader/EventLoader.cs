using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;

namespace Data.Resources.Ability.Loader
{
    public class EventLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public EventLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public Entity MapDataToEntity(EventSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.Name == null) return default;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return default;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsEvent>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.Event });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.Name });

            if (schema.Description != null)
                newEntity.Set<Description>(new Description() { Value = schema.Description });

            return newEntity;
        }
    }
}

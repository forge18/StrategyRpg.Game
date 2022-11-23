using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.MapEvent.Loader
{
    public class MapEventLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public MapEventLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public void MapDataToEntity(MapEventSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.Name == null) return;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsMapEvent>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.ABILITY });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.Name });

            if (schema.Map != null)
            {
                var mapSchema = (MapSchema)schema.Map;
                newEntity.Set<Infrastructure.Ecs.Components.Map>(new Infrastructure.Ecs.Components.Map()
                {
                    Value = mapSchema.DataId
                });
            }

            if (schema.Event != null)
            {
                var eventSchema = (EventSchema)schema.Event;
                newEntity.Set<Event>(new Event() { Value = eventSchema.DataId });
            }

            if (schema.Coordinates != null)
            {
                newEntity.Set<TargetPosition>(new TargetPosition { Value = schema.Coordinates });
            }
        }
    }
}

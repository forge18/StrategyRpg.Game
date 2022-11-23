using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.Unit.Loader
{
    public class UnitLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public UnitLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public void MapDataToEntity(UnitSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.UnitName == null) return;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsUnit>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.ABILITY });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.UnitName });
            newEntity.Set<MoveSpeed>(new MoveSpeed() { Value = schema.MovementSpeed });

            if (schema.Type != null)
            {
                var unitTypeSchema = (UnitTypeSchema)schema.Type;
                newEntity.Set<Infrastructure.Ecs.Components.UnitType>(new Infrastructure.Ecs.Components.UnitType() { Value = unitTypeSchema.DataId });
            }
        }
    }
}

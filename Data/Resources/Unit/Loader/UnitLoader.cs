using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;

namespace Data.Resources.Unit.Loader
{
    public class UnitLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public UnitLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public Entity MapDataToEntity(UnitSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.UnitName == null) return default;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return default;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsUnit>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.Unit });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.UnitName });
            newEntity.Set<MoveSpeed>(new MoveSpeed() { Value = schema.MovementSpeed });

            if (schema.Type != null)
            {
                var unitTypeSchema = (UnitTypeSchema)schema.Type;
                newEntity.Set<Infrastructure.Ecs.Components.UnitType>(new Infrastructure.Ecs.Components.UnitType() { Value = unitTypeSchema.DataId });
            }

            return newEntity;
        }
    }
}

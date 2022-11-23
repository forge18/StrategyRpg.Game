using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.Condition.Loader
{
    public class ConditionLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public ConditionLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public void MapDataToEntity(ConditionSchema schema)
        {
            if (schema == null || schema.DataId == null) return;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsCondition>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.ABILITY });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });

            newEntity.Set<ComparisonOperator>(new ComparisonOperator() { Value = schema.ComparisonOperator });
            newEntity.Set<ComparisonTarget>(new ComparisonTarget() { Value = schema.ComparisonTarget });
            newEntity.Set<ComparisonValue>(new ComparisonValue() { Value = schema.ComparisonValue });

            if (schema.CustomValue != null)
                newEntity.Set<CustomValue>(new CustomValue() { Value = schema.CustomValue });
        }
    }
}

using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;

namespace Data.Resources.Condition.Loader
{
    public class ConditionLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public ConditionLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public Entity MapDataToEntity(ConditionSchema schema)
        {
            if (schema == null || schema.DataId == null) return default;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return default;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsCondition>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.Condition });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });

            newEntity.Set<ComparisonOperator>(new ComparisonOperator() { Value = schema.ComparisonOperator });
            newEntity.Set<ComparisonTarget>(new ComparisonTarget() { Value = schema.ComparisonTarget });
            newEntity.Set<ComparisonValue>(new ComparisonValue() { Value = schema.ComparisonValue });

            if (schema.CustomValue != null)
                newEntity.Set<CustomValue>(new CustomValue() { Value = schema.CustomValue });

            return newEntity;
        }
    }
}

using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs;

namespace Data.Resources.Ability.Loader
{
    public class AbilityLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public AbilityLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public Entity MapDataToEntity(AbilitySchema schema)
        {
            if (schema == null || schema.DataId == null || schema.Name == null) return default;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return default;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsAbility>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.Ability });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.Name });

            newEntity.Set<Cost>(new Cost() { StaminaCost = schema.StaminaCost, ManaCost = schema.ManaCost });
            newEntity.Set<TargetType>(new TargetType() { Value = schema.TargetType });

            if (schema.Description != null)
                newEntity.Set<Description>(new Description() { Value = schema.Description });

            if (schema.Icon != null)
                newEntity.Set<Icon>(new Icon() { Filepath = schema.Icon.ResourcePath });

            if (schema.Conditions.Count > 0)
                newEntity.Set<Conditions>(new Conditions() { Values = ConvertResourcesToDataIdArray(schema.Conditions) });

            if (schema.Effects.Count > 0)
                newEntity.Set<Effects>(new Effects() { Values = ConvertResourcesToDataIdArray(schema.Effects) });

            if (schema.Tags.Count > 0)
                newEntity.Set<Tags>(new Tags() { Values = ConvertTagsArrayToSystemArray(schema.Tags) });

            return newEntity;
        }
    }
}

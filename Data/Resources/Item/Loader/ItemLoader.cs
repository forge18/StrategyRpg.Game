using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.Ability.Loader
{
    public class ItemLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public ItemLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public void MapDataToEntity(ItemSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.Name == null) return;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsItem>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.ABILITY });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.Name });

            newEntity.Set<ShopValue>(new ShopValue() { Value = schema.ShopValue });
            newEntity.Set<DamageType>(new DamageType() { Value = schema.DamageType });
            newEntity.Set<DamageAmount>(new DamageAmount() { Value = schema.DamageAmount });

            if (schema.Description != null)
                newEntity.Set<Description>(new Description() { Value = schema.Description });

            if (schema.Icon != null)
                newEntity.Set<Icon>(new Icon() { Filepath = schema.Icon.ResourcePath });

            if (schema.UnlockableAbility != null)
            {
                var ability = (AbilitySchema)schema.UnlockableAbility;
                newEntity.Set<UnlockableAbility>(new UnlockableAbility() { Value = ability.DataId });
            }

            if (schema.EffectsOnTarget.Count > 0)
                newEntity.Set<EffectsOnTarget>(new EffectsOnTarget() { Values = ConvertResourcesToDataIdArray(schema.EffectsOnTarget) });

            if (schema.EffectsOnSelf.Count > 0)
                newEntity.Set<EffectsOnSelf>(new EffectsOnSelf() { Values = ConvertResourcesToDataIdArray(schema.EffectsOnSelf) });

            if (schema.Tags.Count > 0)
                newEntity.Set<Tags>(new Tags() { Values = ConvertTagsArrayToSystemArray(schema.Tags) });
        }
    }
}

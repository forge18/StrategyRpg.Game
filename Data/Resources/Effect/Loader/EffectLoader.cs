using DefaultEcs;
using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.Ability.Loader
{
    public class EffectLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public EffectLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public Entity MapDataToEntity(EffectSchema schema)
        {
            if (schema == null || schema.DataId == null) return default;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return default;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsEffect>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.Effect });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });

            newEntity.Set<Cooldown>(new Cooldown() { Value = schema.Cooldown });
            newEntity.Set<Duration>(new Duration() { Value = schema.Duration });

            newEntity.Set<Infrastructure.Ecs.Components.Resources>(new Infrastructure.Ecs.Components.Resources
            {
                HealthMax = schema.Health,
                StaminaMax = schema.Stamina,
                ManaMax = schema.Mana
            });

            newEntity.Set<Attributes>(new Attributes
            {
                Power = schema.Power,
                Finesse = schema.Finesse,
                Resolve = schema.Resolve,
                Control = schema.Control
            });

            newEntity.Set<DerivedAttributes>(new DerivedAttributes
            {
                StaminaBasedAttack = schema.StaminaBasedAttack,
                StaminaBasedDefense = schema.StaminaBasedDefense,
                ManaBasedAttack = schema.ManaBasedAttack,
                ManaBasedDefense = schema.ManaBasedDefense,
                CriticalHitChance = schema.CriticalHitChance,
                Speed = schema.Speed
            });

            if (schema.Description != null)
                newEntity.Set<Description>(new Description() { Value = schema.Description });

            if (schema.TagsToApply.Count > 0)
                newEntity.Set<TagsToApply>(new TagsToApply() { Values = ConvertTagsArrayToSystemArray(schema.TagsToApply) });

            if (schema.TagsToRemove.Count > 0)
                newEntity.Set<TagsToRemove>(new TagsToRemove() { Values = ConvertTagsArrayToSystemArray(schema.TagsToRemove) });

            return newEntity;
        }
    }
}

using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.UnitType.Loader
{
    public class UnitTypeLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public UnitTypeLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public void MapDataToEntity(UnitTypeSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.Name == null) return;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsUnitType>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.ABILITY });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.Name });

            if (schema.Description != null)
                newEntity.Set<Description>(new Description() { Value = schema.Description });

            if (schema.Sprite != null)
                newEntity.Set<Sprite>(new Sprite() { Filepath = schema.Sprite.ResourcePath });

            newEntity.Set<Infrastructure.Ecs.Components.Resources>(new Infrastructure.Ecs.Components.Resources
            {
                HealthCurrent = schema.HealthMax,
                HealthMax = schema.HealthMax,
                StaminaCurrent = schema.StaminaMax,
                StaminaMax = schema.StaminaMax,
                ManaCurrent = schema.ManaMax,
                ManaMax = schema.ManaMax
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
                StaminaBasedAttack = 0,
                StaminaBasedDefense = 0,
                ManaBasedAttack = 0,
                ManaBasedDefense = 0,
                CriticalHitChance = 0,
                Speed = 0
            });

            if (schema.WeaponSlot != null)
            {
                var weaponSlot = (AbilitySchema)schema.WeaponSlot;
                newEntity.Set<WeaponSlot>(new WeaponSlot { Value = weaponSlot.DataId });
            }

            if (schema.ArmorSlot != null)
            {
                var armorSlot = (AbilitySchema)schema.ArmorSlot;
                newEntity.Set<ArmorSlot>(new ArmorSlot { Value = armorSlot.DataId });
            }

            if (schema.AccessorySlot != null)
            {
                var accessorySlot = (AbilitySchema)schema.AccessorySlot;
                newEntity.Set<AccessorySlot>(new AccessorySlot { Value = accessorySlot.DataId });
            }

            if (schema.Inventory != null)
            {
                newEntity.Set<Inventory>(new Inventory { Values = ConvertResourcesToDataIdArray(schema.Inventory) });
            }

            if (schema.ActiveAbilities != null)
            {
                newEntity.Set<ActiveAbilities>(new ActiveAbilities
                {
                    Values = ConvertResourcesToDataIdArray(schema.ActiveAbilities)
                });
            }

            if (schema.PassiveAbilities != null)
            {
                newEntity.Set<PassiveAbilities>(new PassiveAbilities
                {
                    Values = ConvertResourcesToDataIdArray(schema.PassiveAbilities)
                });
            }

            if (schema.UnslottedAbilities != null)
            {
                newEntity.Set<UnslottedAbilities>(new UnslottedAbilities
                {
                    Values = ConvertResourcesToDataIdArray(schema.UnslottedAbilities)
                });
            }
        }
    }
}

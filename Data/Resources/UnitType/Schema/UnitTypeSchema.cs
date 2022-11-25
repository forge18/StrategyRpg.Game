using Godot;
using Godot.Collections;

namespace Data.Resources
{
    [Tool]
    public partial class UnitTypeSchema : Resource, IBaseSchema
    {
        [Export] public string DataId;
        [Export] public string Name;
        [Export] public string Description;
        [Export] public Texture2D Sprite;
        [Export] public int HealthMax;
        [Export] public int StaminaMax;
        [Export] public int ManaMax;
        [Export] public int Power;
        [Export] public int Finesse;
        [Export] public int Resolve;
        [Export] public int Control;
        [Export] public Resource WeaponSlot;
        [Export] public Resource ArmorSlot;
        [Export] public Resource AccessorySlot;
        [Export] public Array<Resource> Inventory;
        [Export] public Array<Resource> ActiveAbilities;
        [Export] public Array<Resource> PassiveAbilities;
        [Export] public Array<Resource> UnslottedAbilities;

        public string GetDataId()
        {
            return DataId;
        }

        public System.Collections.Generic.Dictionary<string, SchemaTypeEnum> GetDependencies()
        {
            var dependencies = new System.Collections.Generic.Dictionary<string, SchemaTypeEnum>();

            if (this.WeaponSlot != null)
            {
                var weaponSlot = (ItemSchema)this.WeaponSlot;
                dependencies.Add(weaponSlot.ResourceName, SchemaTypeEnum.Item);
            }

            if (this.ArmorSlot != null)
            {
                var armorSlot = (ItemSchema)this.ArmorSlot;
                dependencies.Add(armorSlot.ResourceName, SchemaTypeEnum.Item);
            }

            if (this.AccessorySlot != null)
            {
                var accessorySlot = (ItemSchema)this.AccessorySlot;
                dependencies.Add(accessorySlot.ResourceName, SchemaTypeEnum.Item);
            }

            foreach (var item in Inventory)
            {
                var inventoryItem = (ItemSchema)item;
                dependencies.Add(inventoryItem.ResourceName, SchemaTypeEnum.Item);
            }

            foreach (var ability in ActiveAbilities)
            {
                var activeAbility = (AbilitySchema)ability;
                dependencies.Add(activeAbility.ResourceName, SchemaTypeEnum.Ability);
            }

            foreach (var ability in PassiveAbilities)
            {
                var passiveAbility = (AbilitySchema)ability;
                dependencies.Add(passiveAbility.ResourceName, SchemaTypeEnum.Ability);
            }

            foreach (var ability in UnslottedAbilities)
            {
                var unslottedAbility = (AbilitySchema)ability;
                dependencies.Add(unslottedAbility.ResourceName, SchemaTypeEnum.Ability);
            }

            return dependencies;
        }
    }
}
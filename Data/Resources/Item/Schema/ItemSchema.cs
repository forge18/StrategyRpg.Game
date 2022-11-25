using Godot;
using Godot.Collections;

namespace Data.Resources
{
    [Tool]
    public partial class ItemSchema : Resource, IBaseSchema
    {
        [Export] public string DataId;
        [Export] public string Name;
        [Export] public string Description;
        [Export] public Texture2D Icon;
        [Export] public EquipmentSlotEnum EquipmentSlot;
        [Export] public int ShopValue;
        [Export] public DamageTypeEnum DamageType;
        [Export] public int DamageAmount;
        [Export] public Array<Resource> EffectsOnTarget;
        [Export] public Array<Resource> EffectsOnSelf;
        [Export] public Resource UnlockableAbility;
        [Export] public Array<string> Tags;

        public string GetDataId()
        {
            return DataId;
        }

        public System.Collections.Generic.Dictionary<string, SchemaTypeEnum> GetDependencies()
        {
            var dependencies = new System.Collections.Generic.Dictionary<string, SchemaTypeEnum>();

            foreach (var effect in EffectsOnTarget)
            {
                var effectItem = (EffectSchema)effect;
                dependencies.Add(effectItem.ResourceName, SchemaTypeEnum.Effect);
            }

            foreach (var effect in EffectsOnSelf)
            {
                var effectItem = (EffectSchema)effect;
                dependencies.Add(effectItem.ResourceName, SchemaTypeEnum.Effect);
            }

            if (this.UnlockableAbility != null)
            {
                var ability = (AbilitySchema)this.UnlockableAbility;
                dependencies.Add(ability.ResourceName, SchemaTypeEnum.Ability);
            }

            return dependencies;
        }
    }
}
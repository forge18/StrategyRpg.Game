using Godot;
using Godot.Collections;

namespace Data.Resources
{
    [Tool]
    public partial class AbilitySchema : Resource, IBaseSchema
    {
        [Export] public string DataId;
        [Export] public string Name;
        [Export] public string Description;
        [Export] public Texture2D Icon;
        [Export] public int StaminaCost;
        [Export] public int ManaCost;
        [Export] public TargetTypeEnum TargetType;
        [Export] public Array<Resource> Conditions;
        [Export] public Array<Resource> Effects;
        [Export] public Array<string> Tags;

        public string GetDataId()
        {
            return DataId;
        }

        public System.Collections.Generic.Dictionary<string, SchemaTypeEnum> GetDependencies()
        {
            var dependencies = new System.Collections.Generic.Dictionary<string, SchemaTypeEnum>();

            foreach (var condition in Conditions)
            {
                var conditionItem = (ConditionSchema)condition;
                dependencies.Add(conditionItem.ResourceName, SchemaTypeEnum.CONDITION);
            }

            foreach (var effect in Effects)
            {
                var effectItem = (EffectSchema)effect;
                dependencies.Add(effectItem.ResourceName, SchemaTypeEnum.EFFECT);
            }

            return dependencies;
        }
    }
}
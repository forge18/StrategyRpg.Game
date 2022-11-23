using Godot;
using Godot.Collections;

namespace Data.Resources
{
    [Tool]
    public partial class EffectSchema : Resource, IBaseSchema
    {
        [Export] public string DataId;
        [Export] public string Description;
        [Export] public int Cooldown;
        [Export] public int Duration;
        [Export] public int Health;
        [Export] public int Stamina;
        [Export] public int Mana;
        [Export] public int Power;
        [Export] public int Finesse;
        [Export] public int Resolve;
        [Export] public int Control;
        [Export] public int StaminaBasedAttack;
        [Export] public int ManaBasedAttack;
        [Export] public int StaminaBasedDefense;
        [Export] public int ManaBasedDefense;
        [Export] public int CriticalHitChance;
        [Export] public int Speed;
        [Export] public Array<string> TagsToApply;
        [Export] public Array<string> TagsToRemove;

        public string GetDataId()
        {
            return DataId;
        }

        public System.Collections.Generic.Dictionary<string, SchemaTypeEnum> GetDependencies()
        {
            var dependencies = new System.Collections.Generic.Dictionary<string, SchemaTypeEnum>();

            return dependencies;
        }
    }
}

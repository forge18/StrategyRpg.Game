using Godot;

namespace Data.Resources
{
    [Tool]
    public partial class ConditionSchema : Resource, IBaseSchema
    {
        [Export] public string DataId;
        [Export] public ComparisonTargetEnum ComparisonTarget;
        [Export] public ComparisonOperatorEnum ComparisonOperator;
        [Export] public ComparisonValueEnum ComparisonValue;
        [Export] public string CustomValue;

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

using Godot;

namespace Data.Resources
{
    public partial class ObjectiveSchema : Resource, IBaseSchema
    {
        [Export] public string DataId;
        [Export] public string Name;
        [Export] public string Description;

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

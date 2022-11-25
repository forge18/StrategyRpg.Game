using System.Collections.Generic;
using Godot;

namespace Data.Resources
{
    [Tool]
    public partial class UnitSchema : Resource, IBaseSchema
    {
        [Export] public string DataId;
        [Export] public string UnitName;
        [Export] public Resource Type;
        [Export] public int MovementSpeed;

        public string GetDataId()
        {
            return DataId;
        }

        public Dictionary<string, SchemaTypeEnum> GetDependencies()
        {
            var dependencies = new Dictionary<string, SchemaTypeEnum>();

            var type = (UnitTypeSchema)this.Type;
            dependencies.Add(type.ResourceName, SchemaTypeEnum.UnitType);

            return dependencies;
        }
    }
}
using Godot;

namespace Data.Resources
{
    public partial class MapEventSchema : Resource, IBaseSchema
    {
        [Export] public string DataId;
        [Export] public string Name;
        [Export] public string Description;

        [Export] public Resource Map;
        [Export] public Vector2 Coordinates;
        [Export] public Resource Event;

        public string GetDataId()
        {
            return DataId;
        }

        public System.Collections.Generic.Dictionary<string, SchemaTypeEnum> GetDependencies()
        {
            var dependencies = new System.Collections.Generic.Dictionary<string, SchemaTypeEnum>();

            if (this.Map != null)
            {
                var map = (MapSchema)this.Map;
                dependencies.Add(map.ResourceName, SchemaTypeEnum.Map);
            }

            if (this.Event != null)
            {
                var eventItem = (EventSchema)this.Event;
                dependencies.Add(eventItem.ResourceName, SchemaTypeEnum.Event);
            }

            return dependencies;
        }
    }
}

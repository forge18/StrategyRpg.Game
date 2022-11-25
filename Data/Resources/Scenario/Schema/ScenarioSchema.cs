using Godot;
using Godot.Collections;

namespace Data.Resources
{
    [Tool]
    public partial class ScenarioSchema : Resource, IBaseSchema
    {
        [Export] public string DataId;
        [Export] public string Name;
        [Export] public string Description;

        [Export] public Resource Map;
        [Export] public Array<Resource> Objectives;
        [Export] public Array<Resource> MapEvents;

        [Export] public Array<Vector2> StartPositions;
        [Export] public Dictionary<Vector2, Resource> UnitPositions;

        public string GetDataId()
        {
            return DataId;
        }

        public System.Collections.Generic.Dictionary<string, SchemaTypeEnum> GetDependencies()
        {
            var dependencies = new System.Collections.Generic.Dictionary<string, SchemaTypeEnum>();

            var mapItem = (MapSchema)Map;
            dependencies.Add(mapItem.ResourceName, SchemaTypeEnum.Map);

            foreach (var objective in Objectives)
            {
                var objectiveItem = (ObjectiveSchema)objective;
                dependencies.Add(objectiveItem.ResourceName, SchemaTypeEnum.Objective);
            }

            foreach (var mapEvent in MapEvents)
            {
                var mapEventItem = (MapEventSchema)mapEvent;
                dependencies.Add(mapEventItem.ResourceName, SchemaTypeEnum.MapEvent);
            }

            foreach (var unitPosition in UnitPositions)
            {
                var unit = (UnitSchema)unitPosition.Value;
                dependencies.Add(unit.ResourceName, SchemaTypeEnum.Unit);

            }

            return dependencies;
        }
    }
}

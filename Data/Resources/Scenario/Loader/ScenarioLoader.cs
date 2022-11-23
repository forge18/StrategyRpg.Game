using Infrastructure.Ecs.Components;
using Infrastructure.Ecs.Entities;

namespace Data.Resources.Scenario.Loader
{
    public class ScenarioLoader : BaseLoader
    {
        private readonly IEcsEntityService _ecsEntityService;

        public ScenarioLoader(IEcsEntityService ecsEntityService)
        {
            _ecsEntityService = ecsEntityService;
        }

        public void MapDataToEntity(ScenarioSchema schema)
        {
            if (schema == null || schema.DataId == null || schema.Name == null) return;
            if (_ecsEntityService.HasSchemaIdInWorld(schema.DataId)) return;

            var newEntity = _ecsEntityService.CreateEntityInWorld();

            newEntity.Set<IsScenario>();
            newEntity.Set<SchemaType>(new SchemaType() { Value = SchemaTypeEnum.ABILITY });
            newEntity.Set<SchemaId>(new SchemaId() { Value = schema.DataId });
            newEntity.Set<Name>(new Name() { Value = schema.Name });

            if (schema.Description != null)
                newEntity.Set<Description>(new Description() { Value = schema.Description });

            if (schema.Map != null)
            {
                var mapSchema = (MapSchema)schema.Map;
                newEntity.Set<Infrastructure.Ecs.Components.Map>(new Infrastructure.Ecs.Components.Map() { Value = mapSchema.DataId });
            }

            if (schema.Objectives.Count > 0)
                newEntity.Set<Objectives>(new Objectives() { Values = ConvertResourcesToDataIdArray(schema.Objectives) });

            if (schema.MapEvents.Count > 0)
                newEntity.Set<MapEvents>(new MapEvents() { Values = ConvertResourcesToDataIdArray(schema.MapEvents) });

            if (schema.StartPositions.Count > 0)
            {
                var positionIndex = 0;
                var positionList = new Godot.Vector2[schema.StartPositions.Count];
                foreach (var position in schema.StartPositions)
                {
                    positionList[positionIndex] = new Godot.Vector2(position.x, position.y);
                    positionIndex++;
                }
                newEntity.Set<MapStartPositions>(new MapStartPositions() { Values = positionList });
            }

            var unitPositions = new Godot.Vector2[schema.UnitPositions.Count];
            var unitIds = new string[schema.UnitPositions.Count];
            var index = 0;
            foreach (var position in schema.UnitPositions)
            {
                var unitSchema = (UnitSchema)position.Value;
                unitPositions[index] = position.Key;
                unitIds[index] = unitSchema.DataId;
                index++;
            }
            newEntity.Set<MapUnitPositions>(new MapUnitPositions { Positions = unitPositions, UnitIds = unitIds });
        }
    }
}

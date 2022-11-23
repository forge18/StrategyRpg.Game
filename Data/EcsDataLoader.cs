using System;
using System.Collections.Generic;
using Data.Resources;
using Data.Resources.Ability.Loader;
using Data.Resources.Condition.Loader;
using Data.Resources.Map.Loader;
using Data.Resources.MapEvent.Loader;
using Data.Resources.Objective.Loader;
using Data.Resources.Scenario.Loader;
using Data.Resources.Unit.Loader;
using Data.Resources.UnitType.Loader;
using Godot;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Queries;

namespace Data
{
    public partial class EcsDataLoader : IEcsDataLoader
    {
        private IEcsEntityService _ecsEntityService;
        private IEcsQueryService _ecsQueryService;

        private Dictionary<string, string> _schemaPaths = new Dictionary<string, string>()
    {
        { "AbilitySchema", "res://Data/Resources/Ability" },
        { "ConditionSchema", "res://Data/Resources/Condition" },
        { "EffectSchema", "res://Data/Resources/Effect" },
        { "EventSchema", "res://Data/Resources/Event" },
        { "ItemSchema", "res://Data/Resources/Item" },
        { "MapSchema", "res://Data/Resources/Map" },
        { "MapEventSchema", "res://Data/Resources/MapEvent" },
        { "ObjectiveSchema", "res://Data/Resources/Objective" },
        { "ScenarioSchema", "res://Data/Resources/Scenario" },
        { "UnitSchema", "res://Data/Resources/Unit" },
        { "UnitTypeSchema", "res://Data/Resources/UnitType" }
    };

        public EcsDataLoader(IEcsEntityService ecsEntityService, IEcsQueryService ecsQueryService)
        {
            _ecsEntityService = ecsEntityService;
            _ecsQueryService = ecsQueryService;
        }

        public void LoadResource<TSchema>(TSchema schema, string resourceName) where TSchema : IBaseSchema
        {
            var filepath = _schemaPaths[schema.GetType().Name] + "/" + resourceName + ".tres";
            var resource = ResourceLoader.Load<IBaseSchema>(filepath);
            var dependencies = resource.GetDependencies();

            LoadDependencies(dependencies);

            switch (schema)
            {
                case AbilitySchema abilitySchema:
                    new AbilityLoader(_ecsEntityService).MapDataToEntity(abilitySchema);
                    break;
                case ConditionSchema conditionSchema:
                    new ConditionLoader(_ecsEntityService).MapDataToEntity(conditionSchema);
                    break;
                case EffectSchema effectSchema:
                    new EffectLoader(_ecsEntityService).MapDataToEntity(effectSchema);
                    break;
                case EventSchema eventSchema:
                    new EventLoader(_ecsEntityService).MapDataToEntity(eventSchema);
                    break;
                case ItemSchema itemSchema:
                    new ItemLoader(_ecsEntityService).MapDataToEntity(itemSchema);
                    break;
                case MapSchema mapSchema:
                    new MapLoader(_ecsEntityService).MapDataToEntity(mapSchema);
                    break;
                case MapEventSchema mapEventSchema:
                    new MapEventLoader(_ecsEntityService).MapDataToEntity(mapEventSchema);
                    break;
                case ObjectiveSchema objectiveSchema:
                    new ObjectiveLoader(_ecsEntityService).MapDataToEntity(objectiveSchema);
                    break;
                case ScenarioSchema scenarioSchema:
                    new ScenarioLoader(_ecsEntityService).MapDataToEntity(scenarioSchema);
                    break;
                case UnitSchema unitSchema:
                    new UnitLoader(_ecsEntityService).MapDataToEntity(unitSchema);
                    break;
                case UnitTypeSchema unitTypeSchema:
                    new UnitTypeLoader(_ecsEntityService).MapDataToEntity(unitTypeSchema);
                    break;
                default:
                    return;
            }
        }

        public void LoadDependencies(Dictionary<string, SchemaTypeEnum> dependencies)
        {
            foreach (var (resourceName, schemaType) in dependencies)
            {

                switch (schemaType)
                {
                    case SchemaTypeEnum.ABILITY:
                        var abilityFilepath = "res://Models/Data/Ability/" + resourceName + ".tres";
                        AbilitySchema abilitySchema = ResourceLoader.Load<AbilitySchema>(abilityFilepath);
                        LoadResource(abilitySchema, resourceName);
                        break;
                    case SchemaTypeEnum.CONDITION:
                        var conditionFilepath = "res://Models/Data/Condition/" + resourceName + ".tres";
                        ConditionSchema conditionSchema = ResourceLoader.Load<ConditionSchema>(conditionFilepath);
                        LoadResource(conditionSchema, resourceName);
                        break;
                    case SchemaTypeEnum.EFFECT:
                        var effectFilepath = "res://Models/Data/Effect/" + resourceName + ".tres";
                        EffectSchema effectSchema = ResourceLoader.Load<EffectSchema>(effectFilepath);
                        LoadResource(effectSchema, resourceName);
                        break;
                    case SchemaTypeEnum.EVENT:
                        var eventFilepath = "res://Models/Data/Event/" + resourceName + ".tres";
                        EventSchema eventSchema = ResourceLoader.Load<EventSchema>(eventFilepath);
                        LoadResource(eventSchema, resourceName);
                        break;
                    case SchemaTypeEnum.ITEM:
                        var itemFilepath = "res://Models/Data/Item/" + resourceName + ".tres";
                        ItemSchema itemSchema = ResourceLoader.Load<ItemSchema>(itemFilepath);
                        LoadResource(itemSchema, resourceName);
                        break;
                    case SchemaTypeEnum.MAP:
                        var mapFilepath = "res://Models/Data/Map/" + resourceName + ".tres";
                        MapSchema mapSchema = ResourceLoader.Load<MapSchema>(mapFilepath);
                        LoadResource(mapSchema, resourceName);
                        break;
                    case SchemaTypeEnum.MAP_EVENT:
                        var mapEventFilepath = "res://Models/Data/MapEvent/" + resourceName + ".tres";
                        MapEventSchema mapEventSchema = ResourceLoader.Load<MapEventSchema>(mapEventFilepath);
                        LoadResource(mapEventSchema, resourceName);
                        break;
                    case SchemaTypeEnum.OBJECTIVE:
                        var objectiveFilepath = "res://Models/Data/Objective/" + resourceName + ".tres";
                        ObjectiveSchema objectiveSchema = ResourceLoader.Load<ObjectiveSchema>(objectiveFilepath);
                        LoadResource(objectiveSchema, resourceName);
                        break;
                    case SchemaTypeEnum.SCENARIO:
                        var scenarioFilepath = "res://Models/Data/Scenario/" + resourceName + ".tres";
                        ScenarioSchema scenarioSchema = ResourceLoader.Load<ScenarioSchema>(scenarioFilepath);
                        LoadResource(scenarioSchema, resourceName);
                        break;
                    case SchemaTypeEnum.UNIT:
                        var unitFilepath = "res://Models/Data/Unit/" + resourceName + ".tres";
                        UnitSchema unitSchema = ResourceLoader.Load<UnitSchema>(unitFilepath);
                        LoadResource(unitSchema, resourceName);
                        break;
                    case SchemaTypeEnum.UNIT_TYPE:
                        var unitTypeFilepath = "res://Models/Data/UnitType/" + resourceName + ".tres";
                        UnitTypeSchema unitTypeSchema = ResourceLoader.Load<UnitTypeSchema>(unitTypeFilepath);
                        LoadResource(unitTypeSchema, resourceName);
                        break;
                    default:
                        throw new Exception("Invalid DataCategoryEnum");
                }
            }
        }
    }
}

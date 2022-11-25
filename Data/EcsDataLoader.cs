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
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Entities;
using Infrastructure.Ecs.Queries;

namespace Data
{
    public partial class EcsDataLoader : IEcsDataLoader
    {
        private IEcsEntityService _ecsEntityService;
        private IEcsQueryService _ecsQueryService;

        private Dictionary<SchemaTypeEnum, string> _schemaPaths = new Dictionary<SchemaTypeEnum, string>()
    {
        { SchemaTypeEnum.Ability, "res://Data/Resources/Ability" },
        { SchemaTypeEnum.Condition, "res://Data/Resources/Condition" },
        { SchemaTypeEnum.Effect, "res://Data/Resources/Effect" },
        { SchemaTypeEnum.Event, "res://Data/Resources/Event" },
        { SchemaTypeEnum.Item, "res://Data/Resources/Item" },
        { SchemaTypeEnum.Map, "res://Data/Resources/Map" },
        { SchemaTypeEnum.MapEvent, "res://Data/Resources/MapEvent" },
        { SchemaTypeEnum.Objective, "res://Data/Resources/Objective" },
        { SchemaTypeEnum.Scenario, "res://Data/Resources/Scenario" },
        { SchemaTypeEnum.Unit, "res://Data/Resources/Unit" },
        { SchemaTypeEnum.UnitType, "res://Data/Resources/UnitType" }
    };

        public EcsDataLoader(IEcsEntityService ecsEntityService, IEcsQueryService ecsQueryService)
        {
            _ecsEntityService = ecsEntityService;
            _ecsQueryService = ecsQueryService;
        }

        public Entity LoadResource(SchemaTypeEnum schema, string resourceName)
        {
            var filepath = _schemaPaths[schema] + "/" + resourceName + ".tres";
            var resource = ResourceLoader.Load<IBaseSchema>(filepath);
            var dependencies = resource.GetDependencies();

            LoadDependencies(dependencies);

            switch (schema)
            {
                case SchemaTypeEnum.Ability:
                    return new AbilityLoader(_ecsEntityService).MapDataToEntity((AbilitySchema)resource);
                case SchemaTypeEnum.Condition:
                    return new ConditionLoader(_ecsEntityService).MapDataToEntity((ConditionSchema)resource);
                case SchemaTypeEnum.Effect:
                    return new EffectLoader(_ecsEntityService).MapDataToEntity((EffectSchema)resource);
                case SchemaTypeEnum.Event:
                    return new EventLoader(_ecsEntityService).MapDataToEntity((EventSchema)resource);
                case SchemaTypeEnum.Item:
                    return new ItemLoader(_ecsEntityService).MapDataToEntity((ItemSchema)resource);
                case SchemaTypeEnum.Map:
                    return new MapLoader(_ecsEntityService).MapDataToEntity((MapSchema)resource);
                case SchemaTypeEnum.MapEvent:
                    return new MapEventLoader(_ecsEntityService).MapDataToEntity((MapEventSchema)resource);
                case SchemaTypeEnum.Objective:
                    return new ObjectiveLoader(_ecsEntityService).MapDataToEntity((ObjectiveSchema)resource);
                case SchemaTypeEnum.Scenario:
                    return new ScenarioLoader(_ecsEntityService).MapDataToEntity((ScenarioSchema)resource);
                case SchemaTypeEnum.Unit:
                    return new UnitLoader(_ecsEntityService).MapDataToEntity((UnitSchema)resource);
                case SchemaTypeEnum.UnitType:
                    return new UnitTypeLoader(_ecsEntityService).MapDataToEntity((UnitTypeSchema)resource);
                default:
                    return default;
            }
        }

        public void LoadDependencies(Dictionary<string, SchemaTypeEnum> dependencies)
        {
            foreach (var (resourceName, schemaType) in dependencies)
            {

                switch (schemaType)
                {
                    case SchemaTypeEnum.Ability:
                        var abilityFilepath = "res://Models/Data/Ability/" + resourceName + ".tres";
                        AbilitySchema abilitySchema = ResourceLoader.Load<AbilitySchema>(abilityFilepath);
                        LoadResource(SchemaTypeEnum.Ability, resourceName);
                        break;
                    case SchemaTypeEnum.Condition:
                        var conditionFilepath = "res://Models/Data/Condition/" + resourceName + ".tres";
                        ConditionSchema conditionSchema = ResourceLoader.Load<ConditionSchema>(conditionFilepath);
                        LoadResource(SchemaTypeEnum.Condition, resourceName);
                        break;
                    case SchemaTypeEnum.Effect:
                        var effectFilepath = "res://Models/Data/Effect/" + resourceName + ".tres";
                        EffectSchema effectSchema = ResourceLoader.Load<EffectSchema>(effectFilepath);
                        LoadResource(SchemaTypeEnum.Effect, resourceName);
                        break;
                    case SchemaTypeEnum.Event:
                        var eventFilepath = "res://Models/Data/Event/" + resourceName + ".tres";
                        EventSchema eventSchema = ResourceLoader.Load<EventSchema>(eventFilepath);
                        LoadResource(SchemaTypeEnum.Event, resourceName);
                        break;
                    case SchemaTypeEnum.Item:
                        var itemFilepath = "res://Models/Data/Item/" + resourceName + ".tres";
                        ItemSchema itemSchema = ResourceLoader.Load<ItemSchema>(itemFilepath);
                        LoadResource(SchemaTypeEnum.Item, resourceName);
                        break;
                    case SchemaTypeEnum.Map:
                        var mapFilepath = "res://Models/Data/Map/" + resourceName + ".tres";
                        MapSchema mapSchema = ResourceLoader.Load<MapSchema>(mapFilepath);
                        LoadResource(SchemaTypeEnum.Map, resourceName);
                        break;
                    case SchemaTypeEnum.MapEvent:
                        var mapEventFilepath = "res://Models/Data/MapEvent/" + resourceName + ".tres";
                        MapEventSchema mapEventSchema = ResourceLoader.Load<MapEventSchema>(mapEventFilepath);
                        LoadResource(SchemaTypeEnum.MapEvent, resourceName);
                        break;
                    case SchemaTypeEnum.Objective:
                        var objectiveFilepath = "res://Models/Data/Objective/" + resourceName + ".tres";
                        ObjectiveSchema objectiveSchema = ResourceLoader.Load<ObjectiveSchema>(objectiveFilepath);
                        LoadResource(SchemaTypeEnum.Objective, resourceName);
                        break;
                    case SchemaTypeEnum.Scenario:
                        var scenarioFilepath = "res://Models/Data/Scenario/" + resourceName + ".tres";
                        ScenarioSchema scenarioSchema = ResourceLoader.Load<ScenarioSchema>(scenarioFilepath);
                        LoadResource(SchemaTypeEnum.Scenario, resourceName);
                        break;
                    case SchemaTypeEnum.Unit:
                        var unitFilepath = "res://Models/Data/Unit/" + resourceName + ".tres";
                        UnitSchema unitSchema = ResourceLoader.Load<UnitSchema>(unitFilepath);
                        LoadResource(SchemaTypeEnum.Unit, resourceName);
                        break;
                    case SchemaTypeEnum.UnitType:
                        var unitTypeFilepath = "res://Models/Data/UnitType/" + resourceName + ".tres";
                        UnitTypeSchema unitTypeSchema = ResourceLoader.Load<UnitTypeSchema>(unitTypeFilepath);
                        LoadResource(SchemaTypeEnum.UnitType, resourceName);
                        break;
                    default:
                        throw new Exception("Invalid DataCategoryEnum");
                }
            }
        }
    }
}

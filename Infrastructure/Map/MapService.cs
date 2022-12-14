using System;
using Data;
using DefaultEcs;
using Godot;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;
using Microsoft.Extensions.DependencyInjection;
using Modules.Combat;
using Presentation.Services;

namespace Infrastructure.Map
{
    public class MapService : IMapService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IEcsDataLoader _ecsDataLoader;
        private readonly INodeTreeService _nodeTreeService;

        public MapService(
            IServiceProvider serviceProvider,
            IMediator mediator,
            IEcsDataLoader ecsDataLoader,
            INodeTreeService nodeTreeService
        )
        {
            _serviceProvider = serviceProvider;
            _mediator = mediator;
            _ecsDataLoader = ecsDataLoader;
            _nodeTreeService = nodeTreeService;
        }

        public void LoadMapEntityIntoEcs(MapEnum mapName)
        {
            _ecsDataLoader.LoadResource(SchemaTypeEnum.Map, mapName.ToString());
        }

        public Entity GetMapEntityFromEcs(MapEnum map)
        {
            var query = (IQuery)ActivatorUtilities.CreateInstance(
                _serviceProvider, typeof(GetMapEntityByEnumQuery),
                new object [] { map }
            );
            var result = _mediator.RunQuery(QueryTypeEnum.GetMapEntityByEnum, query);

            return result.Result.ConvertResultValue<Entity>();
        }

        public PackedScene GetMapScene(MapEnum map)
        {
            var mapEntity = GetMapEntityFromEcs(map);
            ref var mapScene = ref mapEntity.Get<MapScenePath>();

            return ResourceLoader.Load<PackedScene>(mapScene.Value);
        }

        public void LoadMapSceneIntoNodeTree(PackedScene mapScene, Entity mapEntity)
        {
            var mapNode = mapScene.Instantiate();
            var parentNode = _nodeTreeService.GetNode(NodeKeyEnum.Arena);
            _nodeTreeService.AddNodeToTree(mapNode, parentNode);
        }
    }
}
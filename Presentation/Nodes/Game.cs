using DefaultEcs.System;
using Godot;
using Infrastructure.Ecs.Systems;
using Infrastructure.Hub;
using Infrastructure.Hub.EventManagement;
using Infrastructure.Hub.EventManagement.Events;
using Microsoft.Extensions.Logging;

public partial class Game : Node, IEventListener
{
    private readonly IHubMediator _mediator;
    private readonly IEcsSystemService _ecsSystemService;
    private readonly ILogger<Game> _logger;
    private static ISystem<float> _systems;

    // Empty constructor for Godot Engine
    private Game() { }
    public Game(IHubMediator mediator, IEcsSystemService ecsSystemService, ILoggerFactory loggerFactory)
    {
        _mediator = mediator;
        _ecsSystemService = ecsSystemService;
        _logger = loggerFactory.CreateLogger<Game>();
    }

    public override void _Ready()
    {
        _mediator.SubscribeToEvent(EventTypeEnum.EcsSystemsLoaded, this);
    }

    public override void _Process(double delta)
    {
        if (_ecsSystemService.HasSystem())
            _ecsSystemService.UpdateSystems((float)delta);
    }

    public void OnEvent(EventTypeEnum eventType, IEvent eventData)
    {
        if (eventType == EventTypeEnum.EcsSystemsLoaded)
        {
            _logger.LogInformation("Game: EcsSystemsLoaded event received.");
            var gameEvent = (EcsSystemsLoadedEvent)eventData;
            _systems = gameEvent.Systems;
        }
    }
}

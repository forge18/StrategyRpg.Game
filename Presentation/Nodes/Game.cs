using DefaultEcs.System;
using Godot;
using Infrastructure.Ecs;
using Infrastructure.HubMediator;
using Microsoft.Extensions.Logging;

public partial class Game : Node
{
    private readonly IMediator _mediator;
    private readonly IEcsSystemService _ecsSystemService;
    private readonly ILogger<Game> _logger;
    private static ISystem<float> _systems;

    // Empty constructor for Godot Engine
    private Game() { }
    public Game(IMediator mediator, IEcsSystemService ecsSystemService, ILoggerFactory loggerFactory)
    {
        _mediator = mediator;
        _ecsSystemService = ecsSystemService;
        _logger = loggerFactory.CreateLogger<Game>();
    }

    public override void _Ready()
    {
        _ecsSystemService.LoadUnregisteredSystems();
    }

    public override void _Process(double delta)
    {
        if (_ecsSystemService.HasSystems())
            _ecsSystemService.ProcessSystems((float)delta);
    }
}

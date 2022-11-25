using System;
using DefaultEcs.System;
using Godot;
using Infrastructure.MediatorNS;

public partial class Game : Node
{
    private readonly IMediator _mediator;
    private static ISystem<float> _systems;

    // Empty constructor for Godot Engine
    private Game() { }
    public Game(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
        if (_systems != null)
            _systems.Update((float)delta);
    }

}

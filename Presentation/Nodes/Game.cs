using DefaultEcs.System;
using Godot;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

public partial class Game : Node
{
    private static readonly ServiceProvider _serviceProvider = new ContainerBuilder().Build();

    private static ISystem<float> _systems;

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
        if (_systems != null)
            _systems.Update((float)delta);
    }

}

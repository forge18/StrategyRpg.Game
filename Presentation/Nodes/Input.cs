using System;
using System.Collections.Generic;
using Godot;
using Infrastructure.EventManagement;
using Infrastructure.EventManagement.Events;
using Infrastructure.EventManagement.Events.InputUpdated;

namespace Presentation.Nodes
{
    public partial class Input : Node
    {
        private IEventService _eventService;

        private Dictionary<InputButtonsEnum, bool> _buttonStatus = new Dictionary<InputButtonsEnum, bool>
        {
            { InputButtonsEnum.Down, false },
            { InputButtonsEnum.Left, false },
            { InputButtonsEnum.Right, false },
            { InputButtonsEnum.Up, false },
            { InputButtonsEnum.Select, false },
            { InputButtonsEnum.Back, false }
        };

        public void Init(IEventService eventService)
        {
            _eventService = eventService;
        }

        public override void _Input(InputEvent inputEvent)
        {
            _buttonStatus[InputButtonsEnum.Select] = inputEvent.IsActionPressed("Select");

            _buttonStatus[InputButtonsEnum.Back] = inputEvent.IsActionPressed("Back");
        }

        public override void _PhysicsProcess(double delta)
        {
            _buttonStatus[InputButtonsEnum.Up] = Godot.Input.IsActionPressed("Up");

            _buttonStatus[InputButtonsEnum.Right] = Godot.Input.IsActionPressed("Right");

            _buttonStatus[InputButtonsEnum.Down] = Godot.Input.IsActionPressed("Down");

            _buttonStatus[InputButtonsEnum.Left] = Godot.Input.IsActionPressed("Left");

            var buttonData = new InputUpdatedContract(_buttonStatus);
            _eventService.Publish<InputUpdatedContract>(new InputUpdatedEvent(), buttonData);
        }
    }
}
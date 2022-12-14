using System.Collections.Generic;
using Godot;
using Infrastructure.HubMediator;
using Modules.Global;

namespace Presentation.Nodes
{
    public partial class PlayerInput : Node
    {
        private IMediator _mediator;

        private Dictionary<InputButtonsEnum, bool> _buttonStatus = new Dictionary<InputButtonsEnum, bool>
        {
            { InputButtonsEnum.Down, false },
            { InputButtonsEnum.Left, false },
            { InputButtonsEnum.Right, false },
            { InputButtonsEnum.Up, false },
            { InputButtonsEnum.Select, false },
            { InputButtonsEnum.Back, false }
        };

        private PlayerInput() { }

        public PlayerInput(IMediator mediator)
        {
            _mediator = mediator;
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

            var buttonData = new InputUpdatedEvent()
            {
                ButtonStatus = _buttonStatus
            };

            _mediator.NotifyOfEvent(EventTypeEnum.InputUpdated, (IEvent)buttonData);
        }
    }
}
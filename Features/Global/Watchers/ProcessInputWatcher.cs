using System.Collections.Generic;
using DefaultEcs;
using Infrastructure.Ecs;
using Infrastructure.Ecs.Components;
using Infrastructure.HubMediator;

namespace Features.Global
{
    public class ProcessInputWatcher : Watcher, IEventListener
    {
        private readonly IMediator _mediator;

        private readonly World _world;
        private Dictionary<InputButtonsEnum, bool> _inputButtons;

        public ProcessInputWatcher(IEcsWorldService ecsWorldService, IMediator mediator)
        {
            _world = ecsWorldService.GetWorld();
            _mediator = mediator;

            _mediator.SubscribeToEvent(EventTypeEnum.InputUpdated, this);
        }

        public override void Update(float elapsedTime)
        {
            if (_inputButtons.Count > 0)
            {
                UpdateButtonComponents(_inputButtons);
            }
        }

        public void UpdateButtonComponents(Dictionary<InputButtonsEnum, bool> _inputButtons)
        {

            if (_inputButtons.Count > 0)

                if (_inputButtons.ContainsKey(InputButtonsEnum.Down) && _inputButtons[InputButtonsEnum.Down] && !_world.Has<DownKey>())
                    _world.Set<DownKey>();

            if (!_inputButtons[InputButtonsEnum.Down] && _world.Has<DownKey>())
                _world.Remove<DownKey>();

            if (_inputButtons.ContainsKey(InputButtonsEnum.Left) && _inputButtons[InputButtonsEnum.Left] && !_world.Has<LeftKey>())
                _world.Set<LeftKey>();

            if (!_inputButtons[InputButtonsEnum.Left] && _world.Has<LeftKey>())
                _world.Remove<LeftKey>();

            if (_inputButtons.ContainsKey(InputButtonsEnum.Right) && _inputButtons[InputButtonsEnum.Right] && !_world.Has<RightKey>())
                _world.Set<RightKey>();

            if (!_inputButtons[InputButtonsEnum.Right] && _world.Has<RightKey>())
                _world.Remove<RightKey>();

            if (_inputButtons.ContainsKey(InputButtonsEnum.Select) && _inputButtons[InputButtonsEnum.Select] && !_world.Has<SelectKey>())
                _world.Set<SelectKey>();

            if (!_inputButtons[InputButtonsEnum.Select] && _world.Has<SelectKey>())
                _world.Remove<SelectKey>();

            if (_inputButtons.ContainsKey(InputButtonsEnum.Up) && _inputButtons[InputButtonsEnum.Up] && !_world.Has<UpKey>())
                _world.Set<UpKey>();

            if (!_inputButtons[InputButtonsEnum.Up] && _world.Has<UpKey>())
                _world.Remove<UpKey>();
        }

        public void OnEvent(EventTypeEnum eventType, IEvent contract)
        {
            if (eventType == EventTypeEnum.InputUpdated)
            {
                _inputButtons = ((InputUpdatedEvent)contract).ButtonStatus;
            }
        }
    }
}
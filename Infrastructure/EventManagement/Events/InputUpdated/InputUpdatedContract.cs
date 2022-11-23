using System.Collections.Generic;

namespace Infrastructure.EventManagement.Events.InputUpdated
{
    public class InputUpdatedContract : IContract
    {
        Dictionary<InputButtonsEnum, bool> ButtonStatus { get; set; }

        public InputUpdatedContract(Dictionary<InputButtonsEnum, bool> buttonStatus = null)
        {
            if (buttonStatus != null)
            {
                ButtonStatus = buttonStatus;
            }
        }
    }
}
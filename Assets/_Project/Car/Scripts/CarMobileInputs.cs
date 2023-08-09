using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CarMobileInputs : MonoBehaviour
{
    public enum SteerType
    {
        buttons,
        joystick
    }

    [System.Serializable]
    public struct MobileButtons
    {
        public EventTrigger gasInput;
        public EventTrigger brakesInput;
        public EventTrigger leftInput;
        public EventTrigger rightInput;
        public FixedJoystick steerJoystick;

    }

    [SerializeField] private MobileButtons mobileButtons;
    [Space]
    [SerializeField] private CarController carController;
    [SerializeField] private SteerType steerType;
    [Space]
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject joystick;

    private void Awake()
    {
        //mobileButtons.gasInput.triggers.Add(CreateEvent((e) => carController.SetGasInput(1), EventTriggerType.PointerDown));
        //mobileButtons.gasInput.triggers.Add(CreateEvent((e) => carController.SetGasInput(0), EventTriggerType.PointerUp));

        //mobileButtons.brakesInput.triggers.Add(CreateEvent((e) => carController.SetGasInput(-1), EventTriggerType.PointerDown));
        //mobileButtons.brakesInput.triggers.Add(CreateEvent((e) => carController.SetGasInput(0), EventTriggerType.PointerUp));

        //mobileButtons.rightInput.triggers.Add(CreateEvent((e) => carController.SetSteerInput(1), EventTriggerType.PointerDown));
        //mobileButtons.rightInput.triggers.Add(CreateEvent((e) => carController.SetSteerInput(0), EventTriggerType.PointerUp));

        //mobileButtons.leftInput.triggers.Add(CreateEvent((e) => carController.SetSteerInput(-1), EventTriggerType.PointerDown));
        //mobileButtons.leftInput.triggers.Add(CreateEvent((e) => carController.SetSteerInput(0), EventTriggerType.PointerUp));
        
    }

    private EventTrigger.Entry CreateEvent(UnityAction<BaseEventData> action, EventTriggerType type = EventTriggerType.PointerDown)
    {
        var pointerEvent = new EventTrigger.Entry();
        pointerEvent.eventID = type;
        pointerEvent.callback.AddListener(action);
        return pointerEvent;
    }

    public void ChangeSteerType(int type)
    {
        switch (type)
        {
            case (int)SteerType.buttons:
                buttons.SetActive(true);
                joystick.SetActive(false);
                carController.SetVirtualJoystickUse(false);
                break;
            case (int)SteerType.joystick:
                carController.SetVirtualJoystick(mobileButtons.steerJoystick);
                buttons.SetActive(false);
                joystick.SetActive(true);
                carController.SetVirtualJoystickUse(true);
                break;
        }
    }
}

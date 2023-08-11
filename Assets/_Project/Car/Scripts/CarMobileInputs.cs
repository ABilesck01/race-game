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
    [SerializeField] private GameObject joystickContainer;
    [SerializeField] private FixedJoystick joystick;

    private float gasInput;
    private float steerInput;

    private void Awake()
    {
        mobileButtons.gasInput.triggers.Add(CreateEvent((e) => gasInput = 1, EventTriggerType.PointerDown));
        mobileButtons.gasInput.triggers.Add(CreateEvent((e) => gasInput = 0, EventTriggerType.PointerUp));

        mobileButtons.brakesInput.triggers.Add(CreateEvent((e) => gasInput = -1, EventTriggerType.PointerDown));
        mobileButtons.brakesInput.triggers.Add(CreateEvent((e) => gasInput = 0, EventTriggerType.PointerUp));

        mobileButtons.rightInput.triggers.Add(CreateEvent((e) => steerInput = 1, EventTriggerType.PointerDown));
        mobileButtons.rightInput.triggers.Add(CreateEvent((e) => steerInput = 0, EventTriggerType.PointerUp));

        mobileButtons.leftInput.triggers.Add(CreateEvent((e) => steerInput = -1, EventTriggerType.PointerDown));
        mobileButtons.leftInput.triggers.Add(CreateEvent((e) => steerInput = 0, EventTriggerType.PointerUp));
    }

    private void OnEnable()
    {
        CarStats.OnCarSpawn += CarStats_OnCarSpawn;
    }

    private void OnDisable()
    {
        CarStats.OnCarSpawn -= CarStats_OnCarSpawn;
    }

    private void CarStats_OnCarSpawn(object sender, CarStats.OnCarSpawnEventArgs e)
    {
        carController = e.target.GetComponent<CarController>();

        carController.SetInputs(() => gasInput, () => steerInput);
    }

    private EventTrigger.Entry CreateEvent(UnityAction<BaseEventData> action, EventTriggerType type = EventTriggerType.PointerDown)
    {
        var pointerEvent = new EventTrigger.Entry();
        pointerEvent.eventID = type;
        pointerEvent.callback.AddListener(action);
        return pointerEvent;
    }

    private void Update()
    {
        if (steerType != SteerType.joystick) return;

        steerInput = joystick.Horizontal;
    }

    public void ChangeSteerType(int type)
    {
        steerType = (SteerType)type;

        switch (type)
        {
            case (int)SteerType.buttons:
                buttons.SetActive(true);
                joystickContainer.SetActive(false);
                break;
            case (int)SteerType.joystick:
                buttons.SetActive(false);
                joystickContainer.SetActive(true);
                break;
        }
    }
}

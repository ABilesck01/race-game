using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMobileInputs : MonoBehaviour
{
    [System.Serializable]
    public struct MobileButtons
    {
        public EventTrigger gasInput;
        public EventTrigger brakesInput;
        public EventTrigger leftInput;
        public EventTrigger rightInput;
    }

    [SerializeField] private MobileButtons mobileButtons;
    [SerializeField] private CarController carController;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputs : MonoBehaviour
{
    private CarController controller;

    private void Awake()
    {
        controller = GetComponent<CarController>();

        controller.SetInputs(
            () => Input.GetAxis("Vertical"),
            () => Input.GetAxis("Horizontal")
            );
    }
}

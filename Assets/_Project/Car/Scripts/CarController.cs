using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }
    [Serializable]
    public struct Wheel
    {
        public GameObject model;
        public WheelCollider collider;
        public Axel axel;
    }
    [Header("Acceleration")]
    [SerializeField] private float maxAcceleration = 30f;
    [SerializeField] private float brakeAcceleration = 30f;
    [Header("Steer")]
    [SerializeField, Range(0.1f, 2)] private float turnSensitivity = 1f;
    [SerializeField] private float maxSteerAngle = 30f;
    [Space]
    [SerializeField] private Transform centerOfMass;
    [Space]
    [SerializeField] private List<Wheel> wheels;

    private float verticalInput = 0;
    private float horizontalInput = 0;
    private bool isBreaking = false;
    private bool isReverseBreaking = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void getInputs()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        isBreaking = Input.GetButton("Fire1");
        isBreaking = rb.velocity.z > 0 && verticalInput < 0;
    }

    private void Move()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.collider.motorTorque = verticalInput * maxAcceleration * 600 * Time.fixedDeltaTime;
        }
    }

    private void Steer()
    {
        foreach (Wheel wheel in wheels)
        {
             if(wheel.axel == Axel.Front)
            {
                float angle = horizontalInput * turnSensitivity * maxSteerAngle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, angle, 0.6f);
            }
        }
    }

    private void RotateWheels()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheel.model.transform.position = pos;
            wheel.model.transform.rotation = rot;
        }
    }

    private void ApplyBrake()
    {
        if(isBreaking || isReverseBreaking || verticalInput == 0)
        {
            foreach(Wheel wheel in wheels)
            {
                wheel.collider.brakeTorque = 300 * brakeAcceleration * Time.fixedDeltaTime;
            }
            return;
        }

        foreach (Wheel wheel in wheels)
        {
            wheel.collider.brakeTorque = 0;
        }
    }

    private void Update()
    {
        getInputs();
        RotateWheels();
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        ApplyBrake();
    }
}



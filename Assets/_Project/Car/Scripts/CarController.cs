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

    [SerializeField] public CarAsset carAsset;
    [SerializeField] public CarStatsData carStatsData;
    [Space]
    [SerializeField] private Transform centerOfMass;
    [Space]
    [SerializeField] private List<Wheel> wheels;

    private float topSpeed = 33;
    private float maxAcceleration = 30f;
    private float brakeAcceleration = 30f;
    private float turnSensitivity = 1f;
    private float maxSteerAngle = 30f;
    

    private float verticalInput = 0;
    private float horizontalInput = 0;
    private bool isBreaking = false;
    private bool isTopSpeed = false;
    private bool isReverseBreaking = false;
    private CarData carData;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;

        GetStats();
    }

    private void GetStats()
    {
        carData = new CarData(carAsset, carStatsData, 1);

        maxAcceleration = carData.acceleration;
        brakeAcceleration = carData.brakes;
        topSpeed = carData.topSpeed;
        maxSteerAngle = carData.maxSteerAngle;
        turnSensitivity = carData.steerSensitivity;

        rb.mass = carData.mass;

        var joint = new JointSpring();
        joint.spring = carData.SuspensionSpring;
        joint.damper = carData.SuspensionDamper;

        foreach (var wheel in wheels)
        {
            
            wheel.collider.suspensionSpring = joint;

        }
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
        Debug.Log("Move");

        foreach (Wheel wheel in wheels)
        {
            float torque = isTopSpeed ? 0 : verticalInput * maxAcceleration * 600 * Time.fixedDeltaTime;
            wheel.collider.motorTorque = torque;
        }
    }

    private bool IsOnTopSpeed()
    {
        float currentSpeed = rb.velocity.magnitude;
        return currentSpeed >= topSpeed;
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

        isTopSpeed = IsOnTopSpeed();
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        ApplyBrake();
    }
}



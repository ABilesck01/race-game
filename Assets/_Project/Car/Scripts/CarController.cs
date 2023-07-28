using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
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
    [Header("Gears Settings")]
    [SerializeField] private List<AnimationCurve> gearsCurves;
    [SerializeField, Range(0.1f, 0.95f)] private List<float> gearsAutoChange;
    [SerializeField] private bool automaticGeatShift= true;
    [Space]
    [SerializeField] private List<Wheel> wheels;

    private CarData carData;
    private Rigidbody rb;
    
    private float topSpeed = 33;
    private float maxAcceleration = 30f;
    private float brakeAcceleration = 30f;
    private float turnSensitivity = 1f;
    private float maxSteerAngle = 30f;
    

    private float verticalInput = 0;
    private float horizontalInput = 0;
    private bool shiftUp = false;
    private bool shiftDown = false;

    private bool isBreaking = false;
    private bool isTopSpeed = false;
    private bool isReverseBreaking = false;
    private float currentSpeed;
    private int currentGear = 0;
    private float currentAccel = 0;

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

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public float CalculateRpm()
    {
        AnimationCurve currentGearCurve = gearsCurves[currentGear];
        float maxValue = 0f;
        foreach (Keyframe keyframe in currentGearCurve.keys)
        {
            if(keyframe.value > maxValue)
                maxValue = keyframe.value;
        }

        return 1 - (currentAccel / maxValue);
    }

    public float GetNormalizedSpeed()
    {
        return currentSpeed / topSpeed;
    }

    public float GetGearAcceleration()
    {
        return currentAccel;
    }

    private float CalculateGearAccel()
    {
        AnimationCurve currentGearCurve = gearsCurves[currentGear];
        float accel = currentGearCurve.Evaluate(GetNormalizedSpeed());

        return accel;
    }

    private void getInputs()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        isBreaking = Input.GetButton("Fire1");
        shiftUp = Input.GetButtonDown("Fire2");
        shiftDown = Input.GetButtonDown("Fire3");

        isBreaking = rb.velocity.z > 0 && verticalInput < 0;
    }

    private void Move()
    {
        foreach (Wheel wheel in wheels)
        {
            float torque = isTopSpeed ? 0 : verticalInput * maxAcceleration * 600 * Time.fixedDeltaTime * currentAccel;
            wheel.collider.motorTorque = torque;
        }
    }

    private bool IsOnTopSpeed()
    {
        currentSpeed = rb.velocity.magnitude;
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

    private void HandleGearShift()
    {
        if (!automaticGeatShift)
        {
            if(shiftUp)
            {
                if (currentGear >= gearsCurves.Count - 1) return;

                currentGear++;
                return;
            }
            if(shiftDown)
            {
                if (currentGear <= 0) return;

                currentGear--;
            }
            return;
        }

        if(ChangeGearUp())
        {
            if (currentGear >= gearsCurves.Count - 1) return;

            currentGear++;
            return;
        }
        if(ChangeGearDown())
        {
            if (currentGear <= 0) return;

            currentGear--;
        }
    }

    private bool ChangeGearUp()
    {
        switch(currentGear)
        {
            case 0: return GetNormalizedSpeed() > gearsAutoChange[0];
            case 1: return GetNormalizedSpeed() > gearsAutoChange[1];
            case 2: return GetNormalizedSpeed() > gearsAutoChange[2];
            case 3: return GetNormalizedSpeed() > gearsAutoChange[3];
            case 4:
            default: 
                return false;

        }
    }
    private bool ChangeGearDown()
    {
        switch (currentGear)
        {
            case 1: return GetNormalizedSpeed() < gearsAutoChange[0];
            case 2: return GetNormalizedSpeed() < gearsAutoChange[1];
            case 3: return GetNormalizedSpeed() < gearsAutoChange[2];
            case 4: return GetNormalizedSpeed() < gearsAutoChange[3];
            case 0: 
            default:
                return false;

        }
    }

    private void Update()
    {
        getInputs();
        RotateWheels();
        HandleGearShift();

        currentAccel = CalculateGearAccel();
        isTopSpeed = IsOnTopSpeed();
    }

    private void FixedUpdate()
    {
        Move();
        Steer();
        ApplyBrake();
    }
}



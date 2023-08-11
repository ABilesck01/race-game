using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    #region structs

    [System.Serializable]
    public struct WheelsTransform
    {
        public Transform FlWheel;
        public Transform FrWheel;
        public Transform RlWheel;
        public Transform RrWheel;
    }

    [System.Serializable]
    public struct WheelsColliders
    {
        public WheelCollider FlWheel;
        public WheelCollider FrWheel;
        public WheelCollider RlWheel;
        public WheelCollider RrWheel;
    }

    #endregion

    #region variables

    [Header("Wheels")]
    [SerializeField] private WheelsTransform wheelsTransform;
    [SerializeField] private WheelsColliders wheelsColliders;
    [Space]
    [Header("Flags")]
    [SerializeField] private bool keyboardInputs = true;
    [SerializeField] private bool virtualJoystick = false;
    [SerializeField] private Transform centerOfMass;

    private Rigidbody rb;
    private Transform tr;
    private FixedJoystick steerJoystick;


    private CarSettings carSettings;
    private float speed;
    private float speedClamped;
    private float steerPercentage;
    private float currentRpm;
    private float acceleration;
    private int currentGear;

    private Func<float> gasInput;
    private Func<float> steerInput;
    private float brakeInput;



    private bool isReversing = false;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = transform;
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void Update()
    {
        GetKeyboardInputs();
    }

    private void FixedUpdate()
    {
        CalculateNormalizedAcceleration();
        CalculateRpm();
        HandleGears();

        speed = rb.velocity.magnitude;
        if (speed < 0.05) speed = 0;
        //speed = wheelsColliders.RrWheel.rpm * wheelsColliders.RrWheel.radius * 2f * Mathf.PI / 10f;

        ApplyMotorForce();
        ApplySteering();
        ApplyBrake();
        ApplyWheelPositions();
    }

    #endregion

    #region Car methods

    private void GetKeyboardInputs()
    {
        float movingDirection = Vector3.Dot(tr.forward, rb.velocity);

        if (movingDirection < -0.5f && gasInput() > 0)
        {
            brakeInput = Mathf.Abs(gasInput());
        }
        else if (movingDirection > 0.5f && gasInput() < 0)
        {
            brakeInput = Mathf.Abs(gasInput());
        }
        else
        {
            brakeInput = 0;
        }

        isReversing = gasInput() < 0 && movingDirection < 0.5f && speed > 1f;
    }

    private void ApplyMotorForce()
    {
        if(Mathf.Abs(speed) <= carSettings.data.topSpeed)
        {
            wheelsColliders.RlWheel.motorTorque = gasInput() * carSettings.data.motorPower * acceleration;
            wheelsColliders.RrWheel.motorTorque = gasInput() * carSettings.data.motorPower * acceleration;
        }
        else
        {
            wheelsColliders.RlWheel.motorTorque = 0f;
            wheelsColliders.RrWheel.motorTorque = 0f;
        }
    }

    private void ApplySteering()
    {
        steerPercentage = carSettings.data.steeringCurve.Evaluate(GetNormalizedSpeed());

        wheelsColliders.FlWheel.steerAngle = steerPercentage * carSettings.data.maxSteerAngle * steerInput() * carSettings.data.steerSentitivity;
        wheelsColliders.FrWheel.steerAngle = steerPercentage * carSettings.data.maxSteerAngle * steerInput() * carSettings.data.steerSentitivity;
    }

    void ApplyWheelPositions()
    {
        UpdateWheel(wheelsColliders.FlWheel, wheelsTransform.FlWheel);
        UpdateWheel(wheelsColliders.FrWheel, wheelsTransform.FrWheel);
        UpdateWheel(wheelsColliders.RrWheel, wheelsTransform.RrWheel);
        UpdateWheel(wheelsColliders.RlWheel, wheelsTransform.RlWheel);
    }

    private void ApplyBrake()
    {
        wheelsColliders.FrWheel.brakeTorque = brakeInput * carSettings.data.breakePower * 0.7f;
        wheelsColliders.FlWheel.brakeTorque = brakeInput * carSettings.data.breakePower * 0.7f;

        wheelsColliders.RrWheel.brakeTorque = brakeInput * carSettings.data.breakePower * 0.3f;
        wheelsColliders.RlWheel.brakeTorque = brakeInput * carSettings.data.breakePower * 0.3f;
    }

    private void UpdateWheel(WheelCollider coll, Transform wheelMesh)
    {
        //Quaternion quat;
        //Vector3 position;
        coll.GetWorldPose(out Vector3 position, out Quaternion quat);
        if (wheelMesh == null) return;

        wheelMesh.position = position;
        wheelMesh.rotation = quat;
    }

    private void CalculateRpm()
    {
        AnimationCurve accelerationGearCurve = carSettings.gearBox.gearsCurves[currentGear];
        
        float maxValue = 0f;
        Keyframe maxKeyFrame = accelerationGearCurve.keys[0];
        foreach (Keyframe keyframe in accelerationGearCurve.keys)
        {
            if(keyframe.value > maxValue)
            {
                maxValue = keyframe.value;
                maxKeyFrame = keyframe;
            }
        }

        float maxGearSpeed = carSettings.data.topSpeed * maxKeyFrame.time; //get the gear top speed

        float normalizedGearSpeed = (speed * 0.7f) / maxGearSpeed; //normalize the gear top speed based on current speed percentage

        if(normalizedGearSpeed < carSettings.gearBox.minRpm) 
        {
            normalizedGearSpeed = carSettings.gearBox.minRpm;
        }

        currentRpm = normalizedGearSpeed;
    }

    private void CalculateNormalizedAcceleration()
    {
        AnimationCurve accelerationGearCurve = carSettings.gearBox.gearsCurves[currentGear];
        acceleration = accelerationGearCurve.Evaluate(GetNormalizedSpeed());
    }

    private void HandleGears()
    {
        if(ShiftGearUp() && currentGear < carSettings.gearBox.gearsCurves.Length - 1)
        {
            currentGear++;
        }
        else if(ShiftGearDown() && currentGear > 0)
        {
            currentGear--;
        }
    }

    private bool ShiftGearUp()
    {
        switch (currentGear)
        {
            case 0: return GetNormalizedSpeed() > carSettings.gearBox.gearShifts[0];
            case 1: return GetNormalizedSpeed() > carSettings.gearBox.gearShifts[1];
            case 2: return GetNormalizedSpeed() > carSettings.gearBox.gearShifts[2];
            case 3: return GetNormalizedSpeed() > carSettings.gearBox.gearShifts[3];

            default:
            case 4: return false;
        }
    }

    private bool ShiftGearDown()
    {
        switch (currentGear)
        {
            case 1: return GetNormalizedSpeed() < carSettings.gearBox.gearShifts[0];
            case 2: return GetNormalizedSpeed() < carSettings.gearBox.gearShifts[1];
            case 3: return GetNormalizedSpeed() < carSettings.gearBox.gearShifts[2];
            case 4: return GetNormalizedSpeed() < carSettings.gearBox.gearShifts[3];

            default:
            case 0: return false;
        }
    }

    #endregion

    #region Getters

    public float GetCurrentSpeed() => speed;

    public float GetNormalizedRpm() => currentRpm;

    public float GetGasInput() => gasInput();

    public int GetCurrentGear() => currentGear + 1;

    public float GetNormalizedSpeed()
    {
        return speed / carSettings.data.topSpeed;
    }

    public bool GetReverse() => isReversing;

    public float GetSpeedRatio()
    {
        float speed = wheelsColliders.RrWheel.rpm * wheelsColliders.RrWheel.radius * 2f * Mathf.PI / 10f;
        speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);

        var gas = Mathf.Clamp(Mathf.Abs(gasInput()), 0.5f, 1f);
        return speedClamped * gas / (carSettings.data.topSpeed * 3.6f);
    }

    #endregion

    #region Setters

    //public void SetGasInput(float value)
    //{
    //    gasInput = value;
    //}

    //public void SetSteerInput(float value)
    //{
    //    steerInput = value;
    //}

    public void SetInputs(Func<float> gas, Func<float> steer)
    {
        this.gasInput = gas;
        this.steerInput = steer;
    }

    public void SetKeyboardInputs(bool value)
    {
        keyboardInputs = value;
    }

    public void SetVirtualJoystick(FixedJoystick fixedJoystick)
    {
        steerJoystick = fixedJoystick;
    }

    public void SetVirtualJoystickUse(bool value)
    {
        virtualJoystick = value;
    }

    public void SetCarSettings(CarSettings carSettings)
    {
        this.carSettings = carSettings;
    }

    #endregion
}
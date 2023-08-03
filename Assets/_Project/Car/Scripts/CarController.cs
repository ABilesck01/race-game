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

    [System.Serializable]
    public struct CarSettings
    {
        public CarBaseData CarBaseAsset;
        public CarData data;

        public void InitializeBaseData()
        {
            if (CarBaseAsset == null) return;

            data = new CarData(CarBaseAsset);
        }
    }

    #endregion

    #region variables

    [Header("Wheels")]
    [SerializeField] private WheelsTransform wheelsTransform;
    [SerializeField] private WheelsColliders wheelsColliders;
    [SerializeField] private CarSettings carSettings;   
    [Space]
    [Header("Flags")]
    [SerializeField] private bool keyboardInputs = true;
    [SerializeField] private Transform centerOfMass;

    private Rigidbody rb;
    private Transform tr;
    private float speed;
    private float speedClamped;
    private float steerPercentage;

    private float gasInput;
    private float brakeInput;
    private float steerInput;

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

        speed = rb.velocity.magnitude;
        //speed = wheelsColliders.RrWheel.rpm * wheelsColliders.RrWheel.radius * 2f * Mathf.PI / 10f;
        speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);
    }

    private void FixedUpdate()
    {
        ApplyMotorForce();
        ApplySteering();
        ApplyBrake();
        ApplyWheelPositions();
    }

    private void OnValidate()
    {
        carSettings.InitializeBaseData();
    }

    #endregion

    #region Car methods

    private void GetKeyboardInputs()
    {
        if(keyboardInputs)
        {
            gasInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }

        float movingDirection = Vector3.Dot(tr.forward, rb.velocity);

        if (movingDirection < -0.5f && gasInput > 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else if (movingDirection > 0.5f && gasInput < 0)
        {
            brakeInput = Mathf.Abs(gasInput);
        }
        else
        {
            brakeInput = 0;
        }
    }

    private void ApplyMotorForce()
    {
        if(Mathf.Abs(speed) <= carSettings.data.topSpeed)
        {
            wheelsColliders.RlWheel.motorTorque = gasInput * carSettings.data.motorPower;
            wheelsColliders.RrWheel.motorTorque = gasInput * carSettings.data.motorPower;
        }
        else
        {
            wheelsColliders.RlWheel.motorTorque = 0f;
            wheelsColliders.RrWheel.motorTorque = 0f;
        }
    }

    private void ApplySteering()
    {
        steerPercentage = carSettings.data.steeringCurve.Evaluate(NormalizedSpeed());

        wheelsColliders.FlWheel.steerAngle = steerPercentage * carSettings.data.maxSteerAngle * steerInput * carSettings.data.steerSentitivity;
        wheelsColliders.FrWheel.steerAngle = steerPercentage * carSettings.data.maxSteerAngle * steerInput * carSettings.data.steerSentitivity;
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

    #endregion

    #region Getters

    public float GetCurrentSpeed() => speed;

    public float NormalizedSpeed()
    {
        return speed / carSettings.data.topSpeed;
    }

    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(Mathf.Abs(gasInput), 0.5f, 1f);
        return speedClamped * gas / carSettings.data.topSpeed;
    }

    #endregion

    #region Setters

    public void SetGasInput(float value)
    {
        gasInput = value;
    }

    public void SetSteerInput(float value)
    {
        steerInput = value;
    }

    public void SetKeyboardInputs(bool value)
    {
        keyboardInputs = value;
    }

    #endregion

}
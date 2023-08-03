using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    [System.Serializable]
    public struct WheelsTransform
    {
        public Transform FlWheel;
        public Transform FrWheel;
        public Transform RlWheel;
        public Transform RrWheel;
    }

    [Header("Wheels")]
    [SerializeField] private WheelsTransform wheelsTransform;
    [SerializeField] private WheelsColliders wheelsColliders;
    [Space]
    [SerializeField] private Transform centerOfMass;
    [Space]
    [Header("Car Settings")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float motorPower = 500f;
    [SerializeField] private float breakePower = 50000f;
    [SerializeField] private float maxSteerAngle = 35f;
    [SerializeField] private AnimationCurve steeringCurve;

    private Rigidbody rb;
    private Transform tr;
    private float speed;
    private float speedClamped;

    private float gasInput;
    private float brakeInput;
    private float steerInput;

    #region UNITY_METHODS

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = transform;
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void Update()
    {
        GetInputs();

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

    #endregion

    private void GetInputs()
    {
        gasInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

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
        if(Mathf.Abs(speed) <= topSpeed)
        {
            wheelsColliders.RlWheel.motorTorque = gasInput * motorPower;
            wheelsColliders.RrWheel.motorTorque = gasInput * motorPower;
        }
        else
        {
            wheelsColliders.RlWheel.motorTorque = 0f;
            wheelsColliders.RrWheel.motorTorque = 0f;
        }
    }

    private void ApplySteering()
    {
        float steerPercentage = steeringCurve.Evaluate(NormalizedSpeed());

        wheelsColliders.FlWheel.steerAngle = steerPercentage * maxSteerAngle * steerInput;
        wheelsColliders.FrWheel.steerAngle = steerPercentage * maxSteerAngle * steerInput;
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
        wheelsColliders.FrWheel.brakeTorque = brakeInput * breakePower * 0.7f;
        wheelsColliders.FlWheel.brakeTorque = brakeInput * breakePower * 0.7f;

        wheelsColliders.RrWheel.brakeTorque = brakeInput * breakePower * 0.3f;
        wheelsColliders.RlWheel.brakeTorque = brakeInput * breakePower * 0.3f;


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

    public float GetCurrentSpeed() => speedClamped;

    public float NormalizedSpeed()
    {
        return speed / topSpeed;
    }

    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(Mathf.Abs(gasInput), 0.5f, 1f);
        return speedClamped * gas / topSpeed;
    }
}


[System.Serializable]
public struct WheelsColliders
{
    public WheelCollider FlWheel;
    public WheelCollider FrWheel;
    public WheelCollider RlWheel;
    public WheelCollider RrWheel;
}
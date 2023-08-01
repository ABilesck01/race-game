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
        wheelsColliders.RlWheel.motorTorque = gasInput * motorPower;
        wheelsColliders.RrWheel.motorTorque = gasInput * motorPower;
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
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.position = position;
        wheelMesh.rotation = quat;
    }

    private float NormalizedSpeed()
    {
        return speed / topSpeed;
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
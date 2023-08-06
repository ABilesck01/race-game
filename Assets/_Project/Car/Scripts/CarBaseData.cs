using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Assets/Cars/New Base Car")]
public class CarBaseData : ScriptableObject
{
    [Header("Data")]
    public string carName = "";
    public int carValue = 0;
    [Header("values")]
    public float topSpeed;
    public float motorPower = 500f;
    public float breakePower = 50000f;
    public float steerSentitivity = 0.8f;
    public float maxSteerAngle = 35f;
    public AnimationCurve steeringCurve;
    public GearBox gearBox;
}

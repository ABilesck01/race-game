using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Car/New Car")]
public class CarAsset : ScriptableObject
{
    [Header("Car Settings")]
    public float acceleration;
    public float brakes;
    [Tooltip("Top speed in m/s")] public float topSpeed;
    public float maxSteerAngle;
    [Range(0.1f, 2)] public float steerSensitivity;
    [Header("Car physics")]
    public float mass;
    public float SuspensionSpring;
    public float SuspensionDamper;
    public float SuspensionDistance;
}

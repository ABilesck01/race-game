using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Cars/New Car Modifier")]
public class CarModifier : ScriptableObject
{
    public enum ModPart
    {
        topSpeed,
        motorTorque,
        breakPower
    }

    public ModTier modTier;
    public ModPart modPart;
    [Range(0,100)] public int value;
    public float GetValuePercent() => (float)value / 100f;
}
    
public enum ModTier
{
    commom,
    uncommom,
    rare,
    epic,
    legendary
}

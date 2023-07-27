using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Car/New Car Stats")]
public class CarStatsData : ScriptableObject
{
    public StarValue[] Stars;

    public StarValue GetStarLevel(int star)
    {
        return Stars[star - 1];
    }
}

[System.Serializable]
public struct MinMaxValue
{
    public float min; 
    public float max;

    public float getValue()
    {
        return Random.Range(min, max);
    }
}

[System.Serializable]
public struct StarValue
{
    public int Star;
    public MinMaxValue acceleration;
    public MinMaxValue brakes;
    public MinMaxValue topSpeed;
    public MinMaxValue maxSteerAngle;
    public MinMaxValue steerSensitivity;
    public MinMaxValue mass;
    public MinMaxValue SuspensionSpring;
    public MinMaxValue SuspensionDamper;
    public MinMaxValue SuspensionDistance;
}
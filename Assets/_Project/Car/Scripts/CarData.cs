using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarData
{
    public float topSpeed;
    public float motorPower = 500f;
    public float breakePower = 50000f;
    public float steerSentitivity = 0.8f;
    public float maxSteerAngle = 35f;
    public AnimationCurve steeringCurve;

    public CarData(CarBaseData baseData) 
    {
        this.topSpeed = baseData.topSpeed;
        this.motorPower = baseData.motorPower;
        this.breakePower = baseData.breakePower;
        this.steerSentitivity = baseData.steerSentitivity;
        this.maxSteerAngle = baseData.maxSteerAngle;
        this.steeringCurve = baseData.steeringCurve;
    }

}

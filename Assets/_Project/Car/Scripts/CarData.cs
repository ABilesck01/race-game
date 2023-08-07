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
    public List<CarModifier> modfiers;

    public CarData(CarBaseData baseData) 
    {
        this.topSpeed = baseData.topSpeed;
        this.motorPower = baseData.motorPower;
        this.breakePower = baseData.breakePower;
        this.steerSentitivity = baseData.steerSentitivity;
        this.maxSteerAngle = baseData.maxSteerAngle;
        this.steeringCurve = baseData.steeringCurve;
    }

    public void SetModList(List<CarModifier> modfiersToAdd)
    {
        this.modfiers = modfiersToAdd;
        foreach (CarModifier modifier in modfiers)
        {
            switch(modifier.modPart)
            {
                case CarModifier.ModPart.topSpeed:
                    Debug.Log(modifier.GetValuePercent());
                    this.topSpeed += (topSpeed * modifier.GetValuePercent());
                    break;
                case CarModifier.ModPart.motorTorque:
                    Debug.Log(modifier.GetValuePercent());
                    this.motorPower += (motorPower * modifier.GetValuePercent());
                    break;
                case CarModifier.ModPart.breakPower:
                    Debug.Log(modifier.GetValuePercent());
                    this.breakePower += (breakePower * modifier.GetValuePercent());
                    break;
            }
        }
    }

}

using UnityEngine;

[System.Serializable]
public class CarData
{
    public float acceleration;
    public float brakes;
    public float topSpeed;
    public float maxSteerAngle;
    public float steerSensitivity;
    public float mass;
    public float SuspensionSpring;
    public float SuspensionDamper;
    public float SuspensionDistance;

    public CarData(CarAsset baseCar, CarStatsData stats, int stars)
    {
        StarValue starValue = stats.GetStarLevel(stars);

        acceleration = baseCar.acceleration + starValue.acceleration.getValue();
        brakes = baseCar.brakes + starValue.brakes.getValue();
        topSpeed = baseCar.topSpeed + starValue.topSpeed.getValue();
        maxSteerAngle = baseCar.maxSteerAngle + starValue.maxSteerAngle.getValue();
        steerSensitivity = baseCar.steerSensitivity + starValue.steerSensitivity.getValue();
        mass = baseCar.mass + starValue.mass.getValue();
        SuspensionSpring = baseCar.SuspensionSpring + starValue.SuspensionSpring.getValue();
        SuspensionDamper = baseCar.SuspensionDamper + starValue.SuspensionDamper.getValue();
        SuspensionDistance = baseCar.SuspensionDistance + starValue.SuspensionDistance.getValue();
    }
}

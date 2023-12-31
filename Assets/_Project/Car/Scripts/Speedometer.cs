using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public enum SpeedMeasure
    {
        kmh,
        mph
    }

    [SerializeField] private SpeedMeasure measure;
    [SerializeField] private CarController carController;
    [Space]
    [SerializeField] private TextMeshProUGUI txtSpeed;
    [SerializeField] private TextMeshProUGUI txtGear;
    [SerializeField] private Image rpmFill;
    [Space]
    [SerializeField] private float minRpm = 0.1f;

    private void OnEnable()
    {
        CarStats.OnCarSpawn += CarStats_OnCarSpawn;
    }

    private void OnDisable()
    {
        CarStats.OnCarSpawn -= CarStats_OnCarSpawn;
    }

    private void CarStats_OnCarSpawn(object sender, CarStats.OnCarSpawnEventArgs e)
    {
        carController = e.target.GetComponent<CarController>();
    }

    private void LateUpdate()
    {
        if (carController == null) return;

        float speed = carController.GetCurrentSpeed();
        if (measure == SpeedMeasure.kmh)
        {
            speed *= 3.6f;
        }
        else if(measure == SpeedMeasure.mph)
        {
            speed *= 2.24f;
        }

        txtSpeed.text = ((int)speed).ToString();

        int gear = carController.GetCurrentGear();
        if (carController.GetReverse())
            txtGear.text = "R";
        else
            txtGear.text = gear.ToString();

        rpmFill.fillAmount = carController.GetNormalizedRpm();
    }

}



using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public enum SpeedMeasure
    {
        kmh,
        mph
    }

    [SerializeField] private SpeedMeasure measure;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TextMeshProUGUI txtSpeed;

    private void Update()
    {
        float speed = rb.velocity.magnitude;
        if(measure == SpeedMeasure.kmh)
        {
            speed *= 3.6f;
        }
        else if(measure == SpeedMeasure.mph)
        {
            speed *= 2.24f;
        }

        txtSpeed.text = ((int)speed).ToString();
    }

}



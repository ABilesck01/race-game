using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    private CarController controller;
    private AudioSource audioSource;

    private float pitchFromCar;

    private void Awake()
    {
        controller = GetComponent<CarController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        EngineSound();
    }   

    private void EngineSound()
    {
        pitchFromCar = controller.CalculateRpm() + minPitch;

        if( pitchFromCar < minPitch ) 
        {
            audioSource.pitch = minPitch;
        }
        else if( pitchFromCar > maxPitch )
        {
            audioSource.pitch = maxPitch;
        }
        else
        {
            audioSource.pitch = pitchFromCar;
        }

        //if (currentSpeed < minSpeed)
        //{
        //    audioSource.pitch = minPitch;
        //    return;
        //}
        //if (currentSpeed > maxSpeed)
        //{
        //    audioSource.pitch = maxPitch;
        //    return;
        //}

        //if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        //{
        //    audioSource.pitch = minPitch + pitchFromCar;
        //    return;
        //}

        
    }
}

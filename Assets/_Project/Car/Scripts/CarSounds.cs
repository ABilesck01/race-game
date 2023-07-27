using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [Space]
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    private Rigidbody rb;
    private AudioSource audioSource;

    private float currentSpeed;
    private float pitchFromCar;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        EngineSound();
    }   

    private void EngineSound()
    {
        currentSpeed = rb.velocity.magnitude;
        pitchFromCar = rb.velocity.magnitude / 60f;

        if (currentSpeed < minSpeed)
        {
            audioSource.pitch = minPitch;
            return;
        }
        if (currentSpeed > maxSpeed)
        {
            audioSource.pitch = maxPitch;
            return;
        }

        if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {
            audioSource.pitch = minPitch + pitchFromCar;
            return;
        }

        
    }
}

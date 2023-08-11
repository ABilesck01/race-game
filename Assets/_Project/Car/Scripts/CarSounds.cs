using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    public AudioClip idleAudio;
    public AudioClip runningAudio;
    public float minPitch = 0.2f;
    public float maxPitch = 2.0f;
    public float minVolume = 0.2f;
    public float maxVolume = 2.0f;

    private AudioSource audioSource;
    private CarController carController;
    private bool isOnIdle;

    private void Awake()
    {
        carController = GetComponent<CarController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(carController.GetCurrentSpeed() <= 1f && !isOnIdle)
        {
            audioSource.clip = idleAudio;
            isOnIdle = true;
            audioSource.Play();
        }
        else if(carController.GetCurrentSpeed() > 1f && isOnIdle)
        {
            audioSource.clip = runningAudio;
            isOnIdle = false;
            audioSource.Play();
        }

        audioSource.pitch = 1;
        audioSource.volume = 1;

        if (isOnIdle) return;

        float pitch = Mathf.Lerp(minPitch, maxPitch, carController.GetNormalizedRpm() * carController.GetGasInput());
        float volume = Mathf.Lerp(minVolume, maxVolume, carController.GetGasInput());
        audioSource.pitch = pitch;
        audioSource.volume = volume;
    }
}

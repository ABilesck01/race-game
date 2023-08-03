using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    [System.Serializable]
    public struct AudioData
    {
        public AudioSource source;
        public float maxVolume;
        public float maxPitch;
    }

    [SerializeField] private AudioData idleAudioData;
    [SerializeField] private AudioData runningAudioData;
    [SerializeField] private AudioData reveseAudioData;
    [Space]
    [SerializeField] private float limiterSound = 1f;
    [SerializeField] private float limiterFrequency = 3f;
    [SerializeField] private float limiterEngage = 0.8f;

    private CarController carController;
    private float speedRatio = 0;
    private float speedSign = 0;
    private float revLimiter = 0;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    private void Update()
    {
        speedSign = Mathf.Sign(carController.GetSpeedRatio());
        speedRatio = Mathf.Abs(carController.GetSpeedRatio());

        if(speedRatio > limiterEngage)
        {
            revLimiter = (Mathf.Sin(Time.time * limiterFrequency) + 1) * limiterSound * (speedRatio - limiterEngage);
        }

        idleAudioData.source.volume = Mathf.Lerp(0.1f, idleAudioData.maxVolume, speedRatio);

        if(speedSign > 0)
        {
            StopAudio(reveseAudioData);
            HandleAudio(runningAudioData);
        }
        else
        {
            StopAudio(runningAudioData);
            HandleAudio(reveseAudioData);
        }
    }

    private void StopAudio(AudioData audioData)
    {
        audioData.source.volume = 0;
    }

    private void HandleAudio(AudioData data, float minValue = 0.3f)
    {
        data.source.volume = Mathf.Lerp(minValue, data.maxVolume, speedRatio);
        float desiredPitch = Mathf.Lerp(0.3f, data.maxPitch, speedRatio) + revLimiter;
        data.source.pitch = Mathf.Lerp(runningAudioData.source.pitch, desiredPitch, Time.deltaTime);
    }
}

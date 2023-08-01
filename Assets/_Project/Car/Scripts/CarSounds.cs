using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private AudioData runningAudioData;

    private CarController carController;
}

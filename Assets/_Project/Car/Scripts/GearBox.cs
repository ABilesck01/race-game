using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Cars/New Gear Box")]
public class GearBox : ScriptableObject
{
    public AnimationCurve[] gearsCurves;
    [Range(0, 1f)]
    public float[] gearShifts;
    [Range(0.1f, 0.5f)]
    public float minRpm;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Store/New Car Preset")]
public class StoreCarAsset : ScriptableObject
{
    public CarBaseData baseCar;
    [Space]
    public List<CarModifier> commomModifierList;
    public List<CarModifier> uncommomModifierList;
    public List<CarModifier> RareModifierList;
}

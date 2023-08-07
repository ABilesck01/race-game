using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveCarData
{
    public CarBaseData baseData;
    public int appearence;
    public List<CarModifier> modifiersList;
}

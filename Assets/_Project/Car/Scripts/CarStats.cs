using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStats : MonoBehaviour
{
    [SerializeField] private CarSettings carSettings;
    [SerializeField] private MeshFilter appearance;
    private CarController carController;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    public void SetSavedCar(SaveCarData saveCarData)
    {
        carSettings.CarBaseAsset = saveCarData.baseData;
        carSettings.InitializeBaseData();

        carSettings.data.SetModList(saveCarData.modifiersList);

        appearance.mesh = carSettings.CarBaseAsset.appearances[saveCarData.appearence];

        carController.SetCarSettings(carSettings);
    }
}

[System.Serializable]
public struct CarSettings
{
    public CarBaseData CarBaseAsset;
    public CarData data;
    public GearBox gearBox;
    public void InitializeBaseData()
    {
        if (CarBaseAsset == null) return;

        data = new CarData(CarBaseAsset);
        gearBox = CarBaseAsset.gearBox;
    }
}
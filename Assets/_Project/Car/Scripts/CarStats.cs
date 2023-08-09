using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStats : MonoBehaviour
{
    [SerializeField] private CarSettings carSettings;
    [SerializeField] private MeshFilter appearance;
    [SerializeField] private int skinIndex;
    private CarController carController;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    //private void Start()
    //{
    //    InitCar();
    //}

    [ContextMenu("Init car")]
    private void InitCar()
    {
        if (carSettings.CarBaseAsset == null) return;

        carSettings.InitializeBaseData();
        appearance.mesh = carSettings.CarBaseAsset.appearances[skinIndex];

        carController.SetCarSettings(carSettings);
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
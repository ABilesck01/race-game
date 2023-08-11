using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStats : MonoBehaviour
{
    [SerializeField] private CarSettings carSettings;
    [SerializeField] private MeshFilter appearance;
    [SerializeField] private Transform look;
    private int skinIndex;

    private CarController carController;

    public class OnCarSpawnEventArgs : EventArgs
    {
        public Transform target;
        public Transform lookAt;
    }

    public static event EventHandler<OnCarSpawnEventArgs> OnCarSpawn;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    private void Start()
    {
        OnCarSpawn?.Invoke(this, new OnCarSpawnEventArgs
        {
            target = transform,
            lookAt = look,
        });
    }

    [ContextMenu("Init car")]
    private void InitCar()
    {
        if (carSettings.CarBaseAsset == null) return;

        carSettings.InitializeBaseData();
        appearance.mesh = carSettings.CarBaseAsset.appearances[skinIndex];

        carController.SetCarSettings(carSettings);

        OnCarSpawn?.Invoke(this, new OnCarSpawnEventArgs
        {
            target = transform,
            lookAt = look,
        });
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
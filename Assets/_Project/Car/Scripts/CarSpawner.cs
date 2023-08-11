using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    private GameObject car;

    private void OnEnable()
    {
        CarSelection.OnSelectCar += CarSelection_OnSelectCar;
    }

    private void OnDisable()
    {
        CarSelection.OnSelectCar -= CarSelection_OnSelectCar;
    }

    private void Awake()
    {
        LoadCars();
    }
    private void LoadCars()
    {
        var load = SaveSystem.LoadPlayerData();
        SpawnCar(load.getFirstCar());
    }

    private void CarSelection_OnSelectCar(object sender, SaveCarData e)
    {
        SpawnCar(e);
    }

    private void SpawnCar(SaveCarData e)
    {
        var instantiatedCar = Instantiate(e.baseData.carPrefab);
        instantiatedCar.SetSavedCar(e);

        if (car != null)
        {
            Destroy(car);
        }

        car = instantiatedCar.gameObject;
    }
}

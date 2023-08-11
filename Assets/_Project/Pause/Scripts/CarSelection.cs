using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarSelection : MonoBehaviour
{
    [SerializeField] private CarSelectionItem carSelectionItemTemplate;
    [SerializeField] private Transform container;

    private PlayerData playerData;
    private List<SaveCarData> saveCarDatas;
    private GameObject car;

    public static event EventHandler<SaveCarData> OnSelectCar;

    private void Awake()
    {
        LoadCars();
    }

    private void OnEnable()
    {
        SetupItens();
    }

    private void LoadCars()
    {
        var load = SaveSystem.LoadPlayerData();
        playerData = load;
        saveCarDatas = load.GetAllCars();
    }

    private void SetupItens()
    {
        foreach (var car in saveCarDatas)
        {
            var item = Instantiate(carSelectionItemTemplate, container);
            item.SetCar(car, this);
        }
    }

    public void Select(SaveCarData saveCarData)
    {
        OnSelectCar?.Invoke(this, saveCarData);
    }
}

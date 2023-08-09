using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarSelection : MonoBehaviour
{
    [SerializeField] private CarCamera carCamera;
    [Space]
    [SerializeField] private TMP_Dropdown dropdownCars;

    PlayerData playerData;

    private List<SaveCarData> saveCarDatas;
    private GameObject car;

    private void Awake()
    {
        LoadCars();

        dropdownCars.ClearOptions();
        List<string> options = new List<string>();
        foreach (SaveCarData carData in saveCarDatas)
        {
            options.Add(carData.baseData.carName);
        }

        dropdownCars.AddOptions(options);

        //dropdownCars.onValueChanged.AddListener(SelectCar);
    }

    private void Start()
    {
        //SelectCar(0);
    }

    private void LoadCars()
    {
        var load = SaveSystem.LoadPlayerData();
        playerData = load;
        saveCarDatas = load.GetAllCars();
    }

    public void SelectCar(int index)
    {
        Debug.Log("select car");

        if(car != null)
        {
            Destroy(car);
        }

        var carInstance = Instantiate(saveCarDatas[index].baseData.carPrefab, Vector3.zero, Quaternion.identity);
        carInstance.SetSavedCar(saveCarDatas[index]);
        car = carInstance.gameObject;
        carCamera.SetCar(carInstance.GetComponent<Rigidbody>());    
    }
}

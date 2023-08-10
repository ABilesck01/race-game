using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class StoreController : MonoBehaviour
{
    [SerializeField] private List<StoreCarAsset> carAssets;
    [Space]
    [SerializeField] private CarBaseData car;
    [SerializeField] private List<SaveCarData> carsToBuy = new List<SaveCarData>();
    [Space]
    [SerializeField] private Transform position;

    private Dictionary<CarBaseData, StoreCarAsset> cars;

    private int selectedCar = 0;

    private void Awake()
    {
        cars = new Dictionary<CarBaseData, StoreCarAsset>();
        foreach (var car in carAssets)
        {
            cars.Add(car.baseCar, car);
        }
    }

    private void Start()
    {
        StoreData data = StoreData.GetSavedStoreData();
        if(data.NewShuffle())
        {
            ShuffleCarAmount();
            data.date = DateTime.Now;
            data.carsToBuy = this.carsToBuy;

            StoreData.SaveStoreData(data);
        }
        else
        {
            this.carsToBuy = data.carsToBuy;
        }

        SpawnCars();

        SelectCar(selectedCar);
    }

    public void ForceReshuffle()
    {

    }

    [ContextMenu("Suffle")]
    private void ShuffleCarAmount()
    {
        selectedCar = 0;

        carsToBuy.Clear();

        for (int i = 0; i < 3; i++)
        {
            int randModel = Random.Range(0, carAssets.Count);

            var carInstance = ShuffleCar(carAssets[randModel].baseCar, (ModTier)i);
            carsToBuy.Add(carInstance);
        }
    }

    private void ClearCars()
    {
        foreach (Transform car in position)
        {
            Destroy(car.gameObject);
        }
    }

    private void SpawnCars()
    {
        foreach (var car in carsToBuy)
        {
            var carStats = Instantiate(car.baseData.carPrefab, position.position, Quaternion.identity, position);
            carStats.gameObject.SetActive(false);
            carStats.SetSavedCar(car);
        }
    }

    private SaveCarData ShuffleCar(CarBaseData data, ModTier tier)
    {
        StoreCarAsset asset = cars[data];
        List<CarModifier> modifiers = new List<CarModifier>();
        List<CarModifier> modifiersToAdd = new List<CarModifier>();
        int modifiersAmount;
        switch (tier)
        {
            case ModTier.uncommom:
                modifiers = asset.uncommomModifierList;
                modifiersAmount = 2;
                break;
            case ModTier.rare:
                modifiers = asset.RareModifierList;
                modifiersAmount = 3;
                break;
            default:
            case ModTier.commom:
                modifiers = asset.commomModifierList;
                modifiersAmount = 1;
                break;
        }

        while (modifiersToAdd.Count < modifiersAmount)
        {
            int rand = Random.Range(0, 100);
            if(rand < modifiers.Count - 1)
                modifiersToAdd.Add(modifiers[rand]);
        }

        SaveCarData saveCar = new SaveCarData()
        {
            baseData = data,
            appearence = Random.Range(0, data.appearances.Count),
            modifiersList = modifiersToAdd
        };

        return saveCar;
    }

    public void NextCar()
    {
        selectedCar++;
        if (selectedCar >= carsToBuy.Count)
        {
            selectedCar = 0;
        }

        SelectCar(selectedCar);
    }

    public void NextCar(out SaveCarData data)
    {
        selectedCar++;
        if(selectedCar >= carsToBuy.Count)
        {
            selectedCar = 0;
        }

        data = carsToBuy[selectedCar];

        SelectCar(selectedCar);
    }

    public void PrevCar()
    {
        selectedCar--;
        if (selectedCar < 0)
        {
            selectedCar = carsToBuy.Count - 1;
        }


        SelectCar(selectedCar);
    }

    public void PrevCar(out SaveCarData data)
    {
        selectedCar--;
        if (selectedCar < 0)
        {
            selectedCar = carsToBuy.Count - 1;
        }

        data = carsToBuy[selectedCar];

        SelectCar(selectedCar);
    }

    public void SelectCar(int index)
    {
        selectedCar = index;
        for (int i = 0; i < position.childCount; i++)
        {
            position.GetChild(i).gameObject.SetActive(i == selectedCar);
        }
    }

    public void BuyCar()
    {
        var currentData = SaveSystem.LoadPlayerData();

        var boughtCar = carsToBuy[selectedCar];

        currentData.AddSaveCar(boughtCar);

        carsToBuy.Remove(boughtCar);

        Destroy(position.GetChild(selectedCar).gameObject);

        SaveSystem.SavePlayerData(currentData);
        NextCar();
    }
}
[Serializable]
public class StoreData
{
    public DateTime date;
    public string dateString;
    public List<SaveCarData> carsToBuy;

    public bool NewShuffle()
    {
        TimeSpan time = DateTime.Now - this.date;
        Debug.Log(time.Days);
        return time.Days >= 1;
    }
    public StoreData() 
    {
        date = DateTime.Now.AddDays(-1);
        dateString = date.ToString();
        carsToBuy = new List<SaveCarData>();
    }

    public static StoreData GetSavedStoreData()
    {
        if(!PlayerPrefs.HasKey("store"))
            return new StoreData();

        string json = PlayerPrefs.GetString("store");

        var data = JsonUtility.FromJson<StoreData>(json);
        data.date = DateTime.Parse(data.dateString);

        return data;
    }

    public static void SaveStoreData(StoreData data)
    {
        data.dateString = data.date.ToString();

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("store", json);
    }

}
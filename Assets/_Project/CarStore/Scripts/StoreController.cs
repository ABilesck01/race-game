using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
        ShuffleCarAmount();
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
            var carStats = Instantiate(carsToBuy[i].baseData.carPrefab, position.position, Quaternion.identity, position);
            carStats.gameObject.SetActive(false);
            carStats.SetSavedCar(carInstance);
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

        currentData.AddSaveCar(carsToBuy[selectedCar]);

        Destroy(position.GetChild(selectedCar).gameObject);

        SaveSystem.SavePlayerData(currentData);
    }
}

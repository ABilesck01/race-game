using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StoreController : MonoBehaviour
{
    [SerializeField] private List<StoreCarAsset> carAssets;
    [Space]
    [SerializeField] private CarBaseData car;
    [SerializeField] private List<SaveCarData> carsToBuy = new List<SaveCarData>();
    [Space]
    [SerializeField] private Transform[] positions;

    private Dictionary<CarBaseData, StoreCarAsset> cars;

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
        carsToBuy.Clear();
        for (int i = 0; i < 3; i++)
        {
            var carInstance = ShuffleCar(car, (ModTier)i);
            carsToBuy.Add(carInstance);
            var carStats = Instantiate(carsToBuy[i].baseData.carPrefab, positions[i].position, Quaternion.identity, positions[i]);
            carStats.SetSavedCar(carInstance);
        }
    }

    private SaveCarData ShuffleCar(CarBaseData data, ModTier tier)
    {
        StoreCarAsset asset = cars[data];
        List<CarModifier> modifiers = new List<CarModifier>();
        int modifiersAmount = 0;
        List<CarModifier> modifiersToAdd = new List<CarModifier>();
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
}

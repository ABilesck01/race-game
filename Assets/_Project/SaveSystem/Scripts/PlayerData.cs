using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private string playerName;
    [SerializeField] private int currentPoints;
    [SerializeField] private List<SaveCarData> cars;

    public string GetPlayerName() => playerName;

    public int GetCurrentPoints() => currentPoints;

    public List<SaveCarData> GetAllCars() => cars;

    public void AddSaveCar(SaveCarData car)
    {
        if(cars == null)
            cars = new List<SaveCarData>();

        cars.Add(car);
    }

    public SaveCarData getFirstCar() => cars[0];
}

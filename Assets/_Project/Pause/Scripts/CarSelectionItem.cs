using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectionItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtTopSpeed;
    [SerializeField] private TextMeshProUGUI txtAcceleration;
    [SerializeField] private Button btnSelect;

    private SaveCarData saveCarData;

    private CarSelection carSelection;

    public void SetCar(SaveCarData data, CarSelection carSelection)
    {
        CarData car = new CarData(data.baseData);
        car.SetModList(data.modifiersList);

        txtName.text = data.baseData.carName;
        txtTopSpeed.text = (car.topSpeed * 3.6f).ToString("000");
        txtAcceleration.text = car.motorPower.ToString("##000");

        this.saveCarData = data;
        this.carSelection = carSelection;

        btnSelect.onClick.AddListener(SelectCar);
    }

    private void SelectCar()
    {
        carSelection.Select(saveCarData);
    }
}

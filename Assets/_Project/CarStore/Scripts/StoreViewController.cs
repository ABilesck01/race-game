using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreViewController : MonoBehaviour
{
    [System.Serializable]
    public struct CarScreenInfo
    {
        public TextMeshProUGUI txtCarName;
        public TextMeshProUGUI txtTopSpeed;
        public TextMeshProUGUI txtAcceleration;
    }


    [SerializeField] private StoreController storeController;

    [SerializeField] private Button btnNextCar;
    [SerializeField] private Button btnPrevCar;
    [SerializeField] private Button btnBuyCar;
    [SerializeField] private CarScreenInfo screenInfo;

    private void Awake()
    {
        btnNextCar.onClick.AddListener(() =>
        {
            storeController.NextCar(out SaveCarData selectedCar);
            UpdateUI(selectedCar);
        });
        btnPrevCar.onClick.AddListener(() => 
        { 
            storeController.PrevCar(out SaveCarData selectedCar);
            UpdateUI(selectedCar);
        });

        btnBuyCar.onClick.AddListener(() =>
        {
            storeController.BuyCar();
        });
    }

    private void UpdateUI(SaveCarData data)
    {
        CarData carData = new CarData(data.baseData);
        carData.SetModList(data.modifiersList);
        screenInfo.txtCarName.text = data.baseData.carName;
        screenInfo.txtTopSpeed.text = (carData.topSpeed * 3.6f).ToString("000");
        screenInfo.txtAcceleration.text = carData.motorPower.ToString("##000");
    }
}

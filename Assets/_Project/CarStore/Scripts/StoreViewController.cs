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
    [SerializeField] private Button btnShuffle;
    [SerializeField] private GameObject emptyStore;
    [SerializeField] private CarScreenInfo screenInfo;

    private void Awake()
    {
        emptyStore.SetActive(false);

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

        btnShuffle.onClick.AddListener(() =>
        {
            emptyStore.SetActive(false);
            storeController.ForceShuffle();
        });

        StoreController.OnEmptyStore += StoreController_OnEmptyStore;
    }

    private void OnDisable()
    {
        StoreController.OnEmptyStore -= StoreController_OnEmptyStore;
    }

    private void StoreController_OnEmptyStore(object sender, System.EventArgs e)
    {
        emptyStore.SetActive(true);
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

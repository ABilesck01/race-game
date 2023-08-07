using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreViewController : MonoBehaviour
{
    [SerializeField] private StoreController storeController;

    public void BuyCar(int index)
    {
        storeController.BuyCar(index);
    }
}

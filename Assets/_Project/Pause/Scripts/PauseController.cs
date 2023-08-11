using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static readonly string SceneName = "Pause";

    [SerializeField] private Button btnBack;

    private void Awake()
    {
        btnBack.onClick.AddListener(() =>
        {
            SceneManager.UnloadSceneAsync(SceneName);
        });
    }

    private void OnEnable()
    {
        CarSelection.OnSelectCar += CarSelection_OnSelectCar;
    }

    private void OnDisable()
    {
        CarSelection.OnSelectCar -= CarSelection_OnSelectCar;
    }

    private void CarSelection_OnSelectCar(object sender, SaveCarData e)
    {
        SceneManager.UnloadSceneAsync(SceneName);
    }
}

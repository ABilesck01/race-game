using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavigationController : MonoBehaviour
{
    [SerializeField] private Button btnStore;
    [SerializeField] private Button btnWorld;

    private void Awake()
    {
        btnStore.onClick.AddListener(() =>
        {
            SceneManager.UnloadSceneAsync(PauseController.SceneName);
            SceneManager.LoadScene("CarStore");
        });
        btnWorld.onClick.AddListener(() =>
        {
            SceneManager.UnloadSceneAsync(PauseController.SceneName);
            SceneManager.LoadScene("World");
        });
    }
}

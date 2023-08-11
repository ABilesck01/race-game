using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button btnStore;
    [SerializeField] private Button btnCity;

    private void Awake()
    {
        btnStore.onClick.AddListener(() =>
        {
            //SceneManager.UnloadSceneAsync(PauseController.SceneName);
            SceneManager.LoadScene("CarStore");
        });
        btnCity.onClick.AddListener(() =>
        {
            //SceneManager.UnloadSceneAsync(PauseController.SceneName);
            SceneManager.LoadScene("World");
        });
    }
}

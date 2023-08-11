using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private Button btnPause;

    private void Awake()
    {
        btnPause.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(PauseController.SceneName, LoadSceneMode.Additive);
        });
    }
}

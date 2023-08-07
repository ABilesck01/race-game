using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
            SceneManager.LoadScene("CarStore");
        });
        btnCity.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("World");
        });
    }
}

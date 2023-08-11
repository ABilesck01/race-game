using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavBarController : MonoBehaviour
{
    [System.Serializable]
    public struct Tab
    {
        public Button button;
        public GameObject tab;
    }

    [SerializeField] private Tab[] tabs;

    private void Awake()
    {
        

        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;

            tabs[i].button.onClick.AddListener(() =>
            {
                SelectTab(index);
            });
        }
    }

    private void Start()
    {
        SelectTab(0);
    }

    private void SelectTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].tab.SetActive(i == index);
        }
    }
}

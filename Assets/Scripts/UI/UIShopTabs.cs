using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIShopTabs : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;
    [Tooltip("0 - Upgrades, 1 - Background, 2 - Music")]
    [SerializeField] private List<GameObject> shopPages;

    public static Action OnTabChanged;

    private List<Button> tabButtons = new List<Button>();
    private int selectedTab = 0;
    private void Awake()
    {
        Button btn = transform.Find("upgradeBtn").GetComponent<Button>();
        tabButtons.Add(btn);
        btn.onClick.AddListener(() =>
        {
            TabSelected(0, true);
            if(selectedTab != 0)
            {
                selectedTab = 0;
                OnTabChanged?.Invoke();
            }
        });

        btn = transform.Find("bgBtn").GetComponent<Button>();
        tabButtons.Add(btn);
        btn.onClick.AddListener(() =>
        {
            TabSelected(1, true);
            if (selectedTab != 1)
            {
                selectedTab = 1;
                OnTabChanged?.Invoke();
            }
        });

        btn = transform.Find("musicBtn").GetComponent<Button>();
        tabButtons.Add(btn);
        btn.onClick.AddListener(() =>
        {
            TabSelected(2,true);
            if (selectedTab != 2)
            {
                selectedTab = 2;
                OnTabChanged?.Invoke();
            }
        });
    }
    private void Start()
    {
        TabSelected(0, false);
    }
    private void TabSelected(int i, bool playSound)
    {
        foreach (Button button in tabButtons)
        {
            button.GetComponent<Image>().color = deselectedColor;
        }
        tabButtons[i].GetComponent<Image>().color = selectedColor;

        foreach (GameObject go in shopPages)
        {
            go.SetActive(false);
        }
        shopPages[i].SetActive(true);
        if(playSound)
        {
            MusicSoundManager.Instance.PlayUI(GameAssets.Instance.tabSelected);
        }
    }
}

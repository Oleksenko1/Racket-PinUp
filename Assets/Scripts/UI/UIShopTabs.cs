using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIShopTabs : MonoBehaviour
{
    public static UIShopTabs Instance;

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;
    [Tooltip("0 - Upgrades, 1 - Background, 2 - Music")]
    [SerializeField] private List<GameObject> shopPages;
    [SerializeField] private TextMeshProUGUI description;
    [Space(15)]
    [SerializeField]
    [TextArea] private string upgradeDesc;
    [SerializeField]
    [TextArea] private string bgDesc;
    [SerializeField]
    [TextArea] private string musicDesc;

    public Action OnTabChanged;

    private List<Button> tabButtons = new List<Button>();
    private int selectedTab = 0;
    private void Awake()
    {
        Instance = this;

        Button btn = transform.Find("upgradeBtn").GetComponent<Button>();
        tabButtons.Add(btn);
        btn.onClick.AddListener(() =>
        {
            TabSelected(0, true);
            if(selectedTab != 0)
            {
                selectedTab = 0;
                OnTabChanged?.Invoke();
                description.SetText(upgradeDesc);
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
                description.SetText(bgDesc);
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
                description.SetText(musicDesc);
            }
        });
    }
    private void Start()
    {
        TabSelected(0, false);
        description.SetText(upgradeDesc);
    }
    private void TabSelected(int i, bool playSound)
    {
        foreach (Button button in tabButtons)
        {
            button.GetComponent<Image>().color = deselectedColor;
            button.transform.Find("sprite").GetComponent<Image>().color = selectedColor;
        }
        tabButtons[i].GetComponent<Image>().color = selectedColor;
        tabButtons[i].transform.Find("sprite").GetComponent<Image>().color = deselectedColor;

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

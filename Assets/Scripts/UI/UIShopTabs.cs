using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIShopTabs : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;
    [Tooltip("0 - Upgrades, 1 - Background, 2 - Music")]
    [SerializeField] private List<GameObject> shopPages;

    private List<Button> tabButtons = new List<Button>();
    private void Awake()
    {
        Button btn = transform.Find("upgradeBtn").GetComponent<Button>();
        tabButtons.Add(btn);
        btn.onClick.AddListener(() =>
        {
            TabSelected(0, true);
        });

        btn = transform.Find("bgBtn").GetComponent<Button>();
        tabButtons.Add(btn);
        btn.onClick.AddListener(() =>
        {
            TabSelected(1, true);
        });

        btn = transform.Find("musicBtn").GetComponent<Button>();
        tabButtons.Add(btn);
        btn.onClick.AddListener(() =>
        {
            TabSelected(2,true);
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

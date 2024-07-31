using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnhancedShopManagerScript : MonoBehaviour
{
    private enum ItemClass
    {
        upgrade,
        background,
        music
    };

    [SerializeField] private Color selectColor;

    private List<ShopItemSO> upgradeSO = new List<ShopItemSO>();
    private List<ShopItemSO> bgSO = new List<ShopItemSO>();
    private List<ShopItemSO> musicSO = new List<ShopItemSO>();


    [SerializeField] private Transform upgradeContentTab;
    [SerializeField] private Transform bgContentTab;
    [SerializeField] private Transform musicContentTab;

    private TextMeshProUGUI coinsUI;

    // List of all items in the shop
    private List<GameObject> upgradesItemsList = new List<GameObject>();
    private List<GameObject> bgsItemsList = new List<GameObject>();
    private List<GameObject> musicsItemsList = new List<GameObject>();

    // List of all ShopTemplates of every item in the shop
    private List<ShopTemplate> upgradesShopTemplatesList = new List<ShopTemplate>();
    private List<ShopTemplate> backgroundsShopTemplatesList = new List<ShopTemplate>();

    private int selectedBG;

    private void Awake()
    {
        coinsUI = transform.Find("CoinsAmount").GetComponent<TextMeshProUGUI>();

        upgradeSO = GameAssets.Instance.upgradeItemsList.list;
        bgSO = GameAssets.Instance.backgroundItemsList.list;

        selectedBG = PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.SelectedBackGround.ToString(), 0);
    }

    private void Start()
    {
        // Loading upgrade items
        InitUpgrades();

        // Loading background items
        InitBackgrounds();

        TotalCoinsManager.Instance.OnCoinsChanged += UpdateCoinsAmount;
        UpdateCoinsAmount(TotalCoinsManager.Instance.GetCoinsAmount());

        gameObject.SetActive(false);
    }

    private void UpdateCoinsAmount(int coins)
    {
        coinsUI.text = coins.ToString();
    }

    private void LoadCostsAndButtons(ItemClass itemType, int i) // Loading costs and buttons for items
    {
        switch (itemType)
        {
            // FOR UPGRADES
            case ItemClass.upgrade:
                ShopTemplate upgradeShopTemplates = upgradesItemsList[i].GetComponent<ShopTemplate>();
                upgradeShopTemplates.currentLvl.text = "Level " + PlayerPrefs.GetInt(upgradeSO[i].levelSaveName, 0);
                if (upgradeSO[i].levelsCost.Length <= PlayerPrefs.GetInt(upgradeSO[i].levelSaveName, 0)) // If level is maxed - removes ability to upgrade further
                {
                    upgradesShopTemplatesList[i].buyBtn.GetComponent<Image>().color = selectColor;
                    upgradesShopTemplatesList[i].btnTxt.text = "Max lvl";
                    upgradesShopTemplatesList[i].buyBtn.interactable = false;
                    upgradesShopTemplatesList[i].costTxt.text = "";
                    break;
                }
                upgradesShopTemplatesList[i].costTxt.text = upgradeSO[i].levelsCost[PlayerPrefs.GetInt(upgradeSO[i].levelSaveName, 0)].ToString();
                break;

            // FOR BACKGROUNDS
            case ItemClass.background:
                if (PlayerPrefs.GetInt(bgSO[i].levelSaveName, 0) == 1) // If it is bought - show select button
                {
                    backgroundsShopTemplatesList[i].buyBtn.GetComponent<Image>().color = selectColor;
                    if (selectedBG == i) // If background is selected
                    {
                        backgroundsShopTemplatesList[i].btnTxt.text = "Selected";
                        backgroundsShopTemplatesList[i].costTxt.text = "";
                        backgroundsShopTemplatesList[i].buyBtn.interactable = false;
                    }
                    else
                    {
                        backgroundsShopTemplatesList[i].btnTxt.text = "Select";
                        backgroundsShopTemplatesList[i].costTxt.text = "";
                        backgroundsShopTemplatesList[i].buyBtn.interactable = true;
                    }
                    break;
                }
                backgroundsShopTemplatesList[i].costTxt.text = bgSO[i].levelsCost[0].ToString();
                break;

            // FOR MUSIC
            case ItemClass.music:

                break;
        }
    }
    public void UnlockItem(int i)
    {
        ShopItemSO.ItemType itemType = upgradeSO[i].itemType;

        switch (itemType)
        {
            case ShopItemSO.ItemType.RacketAccuracy:
                PlayerPrefs.SetInt(upgradeSO[i].levelSaveName, PlayerPrefs.GetInt(upgradeSO[i].levelSaveName, 0) + 1);

                LoadCostsAndButtons(ItemClass.upgrade, i);
                break;

            case ShopItemSO.ItemType.RacketSize:
                PlayerPrefs.SetInt(upgradeSO[i].levelSaveName, PlayerPrefs.GetInt(upgradeSO[i].levelSaveName, 0) + 1);

                LoadCostsAndButtons(ItemClass.upgrade, i);
                break;

            case ShopItemSO.ItemType.RacketSpeed:
                PlayerPrefs.SetInt(upgradeSO[i].levelSaveName, PlayerPrefs.GetInt(upgradeSO[i].levelSaveName, 0) + 1);

                LoadCostsAndButtons(ItemClass.upgrade, i);
                break;
        }
    }

    public void OpenShop()
    {
        MusicSoundManager.Instance.PlayUI(GameAssets.Instance.basicButton);
        gameObject.SetActive(true);
        UpdateCoinsAmount(TotalCoinsManager.Instance.GetCoinsAmount());
    }
    public void CloseShop()
    {
        MusicSoundManager.Instance.PlayUI(GameAssets.Instance.closeButton);
        gameObject.SetActive(false);
    }

    private void InitUpgrades() // Initializing upgrade items in the shop
    {
        GameObject upgradeItemTemplateGO = GameAssets.Instance.upgradeItemTemplate;
        for (int i = 0; i < upgradeSO.Count; i++)
        {
            GameObject item = Instantiate(upgradeItemTemplateGO, upgradeContentTab);
            ShopTemplate itemInfo = item.GetComponent<ShopTemplate>();
            itemInfo.titleText.text = upgradeSO[i].title;
            itemInfo.descriptionTxt.text = upgradeSO[i].description;
            itemInfo.transform.Find("Icon").GetComponent<Image>().sprite = upgradeSO[i].shopIcon;
            itemInfo.itemIndex = i;

            itemInfo.buyBtn.onClick.AddListener(() =>
            {
                int index = itemInfo.itemIndex;
                Debug.Log("Index is: " + index);
                if (TotalCoinsManager.Instance.DiscardCoins(upgradeSO[index].levelsCost[PlayerPrefs.GetInt(upgradeSO[index].levelSaveName, 0)]))
                {
                    UnlockItem(index);
                    MusicSoundManager.Instance.PlayUI(GameAssets.Instance.itemBought);
                }
                else
                {
                    MusicSoundManager.Instance.PlayUI(GameAssets.Instance.decline);
                    NotEnoughCoinsPopup.Instance.OpenMessageBox();
                }
            });

            upgradesItemsList.Add(item);
            upgradesShopTemplatesList.Add(itemInfo);

            LoadCostsAndButtons(ItemClass.upgrade, i);
        }
    }
    private void InitBackgrounds()
    {
        GameObject backgroundItemTemplateGO = GameAssets.Instance.backgroundItemTemplate;
        for (int i = 0; i < bgSO.Count; i++)
        {
            GameObject item = Instantiate(backgroundItemTemplateGO, bgContentTab);
            ShopTemplate itemInfo = item.GetComponent<ShopTemplate>();
            itemInfo.titleText.text = bgSO[i].title;
            itemInfo.transform.Find("Mask").Find("Icon").GetComponent<Image>().sprite = bgSO[i].shopIcon;
            itemInfo.itemIndex = i;

            if (bgSO[i].levelsCost[0] == 0) // Unlocks item if it's free
            {
                PlayerPrefs.SetInt(bgSO[i].levelSaveName, 1);
            }

            itemInfo.buyBtn.onClick.AddListener(() =>
            {
                int index = itemInfo.itemIndex;
                Debug.Log("Index is: " + index);
                if (PlayerPrefs.GetInt(bgSO[index].levelSaveName, 0) == 0) // If background is not bought - act as 'Buy' button
                {
                    if (TotalCoinsManager.Instance.DiscardCoins(bgSO[index].levelsCost[0])) // If enough money - buy this item
                    {
                        MusicSoundManager.Instance.PlayUI(GameAssets.Instance.itemBought);
                        PlayerPrefs.SetInt(bgSO[index].levelSaveName, 1);

                        LoadCostsAndButtons(ItemClass.background, index);
                    }
                    else
                    {
                        MusicSoundManager.Instance.PlayUI(GameAssets.Instance.decline);
                        NotEnoughCoinsPopup.Instance.OpenMessageBox();
                    }
                }
                else // If background is bought and not selected. On click this bg will be selected
                {
                    selectedBG = index;
                    PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.SelectedBackGround.ToString(), index);
                    for(int x = 0; x < bgSO.Count; x++)
                    {
                        LoadCostsAndButtons(ItemClass.background, x);
                    }
                    MusicSoundManager.Instance.PlayUI(GameAssets.Instance.basicButton);
                }
            });
            bgsItemsList.Add(item);
            backgroundsShopTemplatesList.Add(itemInfo);

            LoadCostsAndButtons(ItemClass.background, i);
        }
    }
}

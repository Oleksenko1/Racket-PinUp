using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
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
    [SerializeField] private Color textSelectColor;

    public event Action OnShopClose;

    // Contains list of all SO of items
    private List<ShopItemSO> upgradeSO = new List<ShopItemSO>();
    private List<ShopItemSO> bgSO = new List<ShopItemSO>();
    private List<ShopItemSO> musicSO = new List<ShopItemSO>();

    // Contains data where items should be placed in the shop
    [SerializeField] private Transform upgradeContentTab;
    [SerializeField] private Transform bgContentTab;
    [SerializeField] private Transform musicContentTab;

    // Sprites for music items in the shop
    [Space(15)]
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;

    private TextMeshProUGUI coinsUI;

    // List of all items in the shop
    private List<GameObject> upgradesItemsList = new List<GameObject>();
    private List<GameObject> bgsItemsList = new List<GameObject>();
    private List<GameObject> musicsItemsList = new List<GameObject>();

    // List of all ShopTemplates of every item in the shop
    private List<ShopTemplate> upgradesShopTemplatesList = new List<ShopTemplate>();
    private List<ShopTemplate> backgroundsShopTemplatesList = new List<ShopTemplate>();
    private List<ShopTemplate> musicsShopTemplatesList = new List<ShopTemplate>();

    private int selectedBG;
    private int selectedMusic;

    private void Awake()
    {
        coinsUI = transform.Find("CoinsAmount").GetComponent<TextMeshProUGUI>();

        upgradeSO = GameAssets.Instance.upgradeItemsList.list;
        bgSO = GameAssets.Instance.backgroundItemsList.list;
        musicSO = GameAssets.Instance.musicItemsList.list;

        selectedBG = PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.SelectedBackGround.ToString(), 0);
        selectedMusic = PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.SelectedMusic.ToString(), 0);
    }

    private void Start()
    {
        // Loading upgrade items
        InitUpgrades();

        // Loading background items
        InitBackgrounds();

        // Loading music items
        InitMusics();

        OnShopClose += UpdateMusicItems;
        UIShopTabs.Instance.OnTabChanged += UpdateMusicItems;

        TotalCoinsManager.Instance.OnCoinsChanged += UpdateCoinsAmount;
        UpdateCoinsAmount(TotalCoinsManager.Instance.GetCoinsAmount());

        if(PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.OpenShopOnStart.ToString(), 0) == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.OpenShopOnStart.ToString(), 0);
        }
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
                    upgradesShopTemplatesList[i].btnTxt.color = textSelectColor;
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
                        backgroundsShopTemplatesList[i].buyBtn.GetComponent<Image>().color = selectColor;
                        backgroundsShopTemplatesList[i].btnTxt.color = textSelectColor;
                        backgroundsShopTemplatesList[i].costTxt.text = "";
                        backgroundsShopTemplatesList[i].buyBtn.interactable = false;
                    }
                    else
                    {
                        backgroundsShopTemplatesList[i].btnTxt.text = "Select";
                        backgroundsShopTemplatesList[i].btnTxt.color = textSelectColor;
                        backgroundsShopTemplatesList[i].costTxt.text = "";
                        backgroundsShopTemplatesList[i].buyBtn.interactable = true;
                    }
                    break;
                }
                backgroundsShopTemplatesList[i].costTxt.text = bgSO[i].levelsCost[0].ToString();
                break;

            // FOR MUSIC
            case ItemClass.music:

                for (int x = 0; x < musicsItemsList.Count; x++)
                {
                    musicsShopTemplatesList[x].playBtn.transform.Find("image").GetComponent<Image>().sprite = playSprite;
                    if (musicSO[x].music == MusicSoundManager.Instance.CurrentMusic()) // If item is currently playing
                    {
                        musicsShopTemplatesList[x].playBtn.transform.Find("image").GetComponent<Image>().sprite = pauseSprite;
                    }
                }

                if (PlayerPrefs.GetInt(musicSO[i].levelSaveName, 0) == 1) // If music is bought
                {
                    musicsShopTemplatesList[i].buyBtn.GetComponent<Image>().color = selectColor;
                    if (selectedMusic == i) // If music is selected
                    {
                        musicsShopTemplatesList[i].btnTxt.text = "Selected";
                        musicsShopTemplatesList[i].buyBtn.GetComponent<Image>().color = selectColor;
                        musicsShopTemplatesList[i].btnTxt.color = textSelectColor;
                        musicsShopTemplatesList[i].costTxt.text = "";
                        musicsShopTemplatesList[i].buyBtn.interactable = false;
                    }
                    else
                    {
                        musicsShopTemplatesList[i].btnTxt.text = "Select";
                        musicsShopTemplatesList[i].btnTxt.color = textSelectColor;
                        musicsShopTemplatesList[i].costTxt.text = "";
                        musicsShopTemplatesList[i].buyBtn.interactable = true;
                    }
                    break;
                }
                musicsShopTemplatesList[i].costTxt.text = musicSO[i].levelsCost[0].ToString();

                break;
        }
    }
    public void UpgradeItem(int i)
    {
        PlayerPrefs.SetInt(upgradeSO[i].levelSaveName, PlayerPrefs.GetInt(upgradeSO[i].levelSaveName, 0) + 1);

        LoadCostsAndButtons(ItemClass.upgrade, i);
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
                    UpgradeItem(index);
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

                        ChooseBG(index);
                    }
                    else
                    {
                        MusicSoundManager.Instance.PlayUI(GameAssets.Instance.decline);
                        NotEnoughCoinsPopup.Instance.OpenMessageBox();
                    }
                }
                else // If background is bought and not selected. On click this bg will be selected
                {
                    ChooseBG(index);
                    MusicSoundManager.Instance.PlayUI(GameAssets.Instance.basicButton);
                }
            });
            bgsItemsList.Add(item);
            backgroundsShopTemplatesList.Add(itemInfo);

            LoadCostsAndButtons(ItemClass.background, i);
        }
    }
    private void InitMusics()
    {
        GameObject musicItemTemplateGO = GameAssets.Instance.musicItemTemplate;
        for (int i = 0; i < musicSO.Count; i++)
        {
            GameObject item = Instantiate(musicItemTemplateGO, musicContentTab);
            ShopTemplate itemInfo = item.GetComponent<ShopTemplate>();
            itemInfo.titleText.text = musicSO[i].title;
            itemInfo.itemIndex = i;

            if (musicSO[i].levelsCost[0] == 0) // Unlocks item if it's free
            {
                PlayerPrefs.SetInt(musicSO[i].levelSaveName, 1);
            }

            // Buy button
            itemInfo.buyBtn.onClick.AddListener(() =>
            {
                int index = itemInfo.itemIndex;
                Debug.Log("Index is: " + index);
                if (PlayerPrefs.GetInt(musicSO[index].levelSaveName, 0) == 0) // If music is not bought - act as 'Buy' button
                {
                    if (TotalCoinsManager.Instance.DiscardCoins(musicSO[index].levelsCost[0])) // If enough money - buy this item
                    {
                        MusicSoundManager.Instance.PlayUI(GameAssets.Instance.itemBought);
                        PlayerPrefs.SetInt(musicSO[index].levelSaveName, 1);

                        ChooseMusic(index);
                    }
                    else
                    {
                        MusicSoundManager.Instance.PlayUI(GameAssets.Instance.decline);
                        NotEnoughCoinsPopup.Instance.OpenMessageBox();
                    }
                }
                else // If music is bought and not selected. On click this music will be selected
                {
                    ChooseMusic(index);
                    MusicSoundManager.Instance.PlayUI(GameAssets.Instance.basicButton);
                }
            });


            // Play music button
            itemInfo.playBtn.onClick.AddListener(() =>
            {
                int index = itemInfo.itemIndex;
                if (MusicSoundManager.Instance.CurrentMusic() != musicSO[index].music) // If this music is not playing currently
                {
                    MusicSoundManager.Instance.PlayMusic(musicSO[index].music);
                    LoadCostsAndButtons(ItemClass.music, index);
                }
                else
                {
                    itemInfo.playBtn.transform.Find("image").GetComponent<Image>().sprite = playSprite;
                    MusicSoundManager.Instance.PlayMusic(GameAssets.Instance.menuMusic);
                }
            });


            musicsItemsList.Add(item);
            musicsShopTemplatesList.Add(itemInfo);

            LoadCostsAndButtons(ItemClass.music, i);
        }
    }
    private void UpdateMusicItems()
    {
        if (MusicSoundManager.Instance.CurrentMusic() != GameAssets.Instance.menuMusic) // Plays menu music when closed, and turns off every other music in the shop
        {
            MusicSoundManager.Instance.PlayMusic(GameAssets.Instance.menuMusic);
        }
        foreach (ShopTemplate so in musicsShopTemplatesList) // Sets every item to have play icon
        {
            so.playBtn.transform.Find("image").GetComponent<Image>().sprite = playSprite;
        }
    }
    private void ChooseBG(int index)
    {
        selectedBG = index;
        PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.SelectedBackGround.ToString(), index);
        for (int x = 0; x < bgSO.Count; x++)
        {
            LoadCostsAndButtons(ItemClass.background, x);
        }
    }
    private void ChooseMusic(int index)
    {
        selectedMusic = index;
        PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.SelectedMusic.ToString(), index);
        for (int x = 0; x < musicSO.Count; x++)
        {
            LoadCostsAndButtons(ItemClass.music, x);
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
        OnShopClose?.Invoke();
        gameObject.SetActive(false);
    }
}

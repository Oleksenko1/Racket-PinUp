using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManagerScript : MonoBehaviour
{
    public static ShopManagerScript Instance;
    public TMP_Text coinsUi;
    [Header("Upgrades")]
    public ShopItemSO[] upgradeShopItemsSO;
    public GameObject[] upgradeShopPanelsGO;
    public ShopTemplate[] upgradeShopPanels;
    public Button[] upgradePurchaseBtns;
    [Header("Backgrounds")]
    public ShopItemSO[] bgShopItemsSO;
    public GameObject[] bgShopPanelsGO;
    public ShopTemplate[] bgShopPanels;
    public Button[] bgPurchaseBtns;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < upgradeShopItemsSO.Length; i++)
        {
            upgradeShopPanelsGO[i].SetActive(true);
        }
        TotalCoinsManager.Instance.OnCoinsChanged += UpdateCoinsAmount;
        UpdateCoinsAmount(TotalCoinsManager.Instance.GetCoinsAmount());

        LoadUpgradePanels();

        gameObject.SetActive(false);
    }

    private void UpdateCoinsAmount(int coins)
    {
        coinsUi.text = coins.ToString();
    }
    private void LoadUpgradePanels() // Loads panels
    {
        for(int i = 0; i < upgradeShopItemsSO.Length; i++)
        {
            upgradeShopPanels[i].titleText.text = upgradeShopItemsSO[i].title;
            upgradeShopPanels[i].descriptionTxt.text = upgradeShopItemsSO[i].description;
            upgradeShopPanelsGO[i].transform.Find("Icon").GetComponent<Image>().sprite = upgradeShopItemsSO[i].shopIcon;

            LoadUpgradesCostAndButton(i);
        }
    }
    private void LoadUpgradesCostAndButton(int btnNo) // Loads buttons and costs
    {
        upgradeShopPanels[btnNo].currentLvl.text = "Level " + PlayerPrefs.GetInt(upgradeShopItemsSO[btnNo].levelSaveName, 0);
        if (upgradeShopItemsSO[btnNo].levelsCost.Length <= PlayerPrefs.GetInt(upgradeShopItemsSO[btnNo].levelSaveName, 0)) // If level is maxed - removes ability to upgrade further
        {
            upgradePurchaseBtns[btnNo].transform.Find("PurchaseBtn txt").GetComponent<TextMeshProUGUI>().text = "Max lvl";
            upgradePurchaseBtns[btnNo].interactable = false;
            upgradeShopPanels[btnNo].costTxt.text = "";
            return;
        }
        upgradeShopPanels[btnNo].costTxt.text = upgradeShopItemsSO[btnNo].levelsCost[PlayerPrefs.GetInt(upgradeShopItemsSO[btnNo].levelSaveName, 0)].ToString();
    }
    public void PurchaseItem(int btnNo)
    {
        if(TotalCoinsManager.Instance.DiscardCoins(upgradeShopItemsSO[btnNo].levelsCost[PlayerPrefs.GetInt(upgradeShopItemsSO[btnNo].levelSaveName, 0)]))
        {
            UnlockItem(btnNo);
            MusicSoundManager.Instance.PlayUI(GameAssets.Instance.itemBought);
        }
        else
        {
            MusicSoundManager.Instance.PlayUI(GameAssets.Instance.decline);
            NotEnoughCoinsPopup.Instance.OpenMessageBox();
        }
    }
    public void UnlockItem(int btnNo)
    {
        ShopItemSO.ItemType itemType = upgradeShopItemsSO[btnNo].itemType;

        switch (itemType)
        {
            case ShopItemSO.ItemType.RacketAccuracy:
                PlayerPrefs.SetInt(upgradeShopItemsSO[btnNo].levelSaveName, PlayerPrefs.GetInt(upgradeShopItemsSO[btnNo].levelSaveName, 0) + 1);
                break;

            case ShopItemSO.ItemType.RacketSize:
                PlayerPrefs.SetInt(upgradeShopItemsSO[btnNo].levelSaveName, PlayerPrefs.GetInt(upgradeShopItemsSO[btnNo].levelSaveName, 0) + 1);
                break;

            case ShopItemSO.ItemType.RacketSpeed:
                PlayerPrefs.SetInt(upgradeShopItemsSO[btnNo].levelSaveName, PlayerPrefs.GetInt(upgradeShopItemsSO[btnNo].levelSaveName, 0) + 1);
                break;
        }
        LoadUpgradesCostAndButton(btnNo);
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
}

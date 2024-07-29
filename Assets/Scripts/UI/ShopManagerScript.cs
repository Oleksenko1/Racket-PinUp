using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManagerScript : MonoBehaviour
{
    public static ShopManagerScript Instance;
    public TMP_Text coinsUi;
    public ShopItemSO[] shopItemsSO;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }
        TotalCoinsManager.Instance.OnCoinsChanged += UpdateCoinsAmount;
        UpdateCoinsAmount(TotalCoinsManager.Instance.GetCoinsAmount());

        LoadPanels();

        gameObject.SetActive(false);
    }

    private void UpdateCoinsAmount(int coins)
    {
        coinsUi.text = coins.ToString();
    }
    private void LoadPanels() // Loads panels
    {
        for(int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].titleText.text = shopItemsSO[i].title;
            shopPanels[i].descriptionTxt.text = shopItemsSO[i].description;
            shopPanelsGO[i].transform.Find("Icon").GetComponent<Image>().sprite = shopItemsSO[i].shopIcon;

            LoadCostAndButton(i);
        }
    }
    private void LoadCostAndButton(int btnNo) // Loads buttons and costs
    {
        shopPanels[btnNo].currentLvl.text = "Level " + PlayerPrefs.GetInt(shopItemsSO[btnNo].levelSaveName, 0);
        if (shopItemsSO[btnNo].levelsCost.Length <= PlayerPrefs.GetInt(shopItemsSO[btnNo].levelSaveName, 0)) // If level is maxed - removes ability to upgrade further
        {
            myPurchaseBtns[btnNo].transform.Find("PurchaseBtn txt").GetComponent<TextMeshProUGUI>().text = "Max lvl";
            myPurchaseBtns[btnNo].interactable = false;
            shopPanels[btnNo].costTxt.text = "";
            return;
        }
        shopPanels[btnNo].costTxt.text = shopItemsSO[btnNo].levelsCost[PlayerPrefs.GetInt(shopItemsSO[btnNo].levelSaveName, 0)].ToString();
    }
    public void PurchaseItem(int btnNo)
    {
        if(TotalCoinsManager.Instance.DiscardCoins(shopItemsSO[btnNo].levelsCost[PlayerPrefs.GetInt(shopItemsSO[btnNo].levelSaveName, 0)]))
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
        ShopItemSO.ItemType itemType = shopItemsSO[btnNo].itemType;

        switch (itemType)
        {
            case ShopItemSO.ItemType.RacketAccuracy:
                PlayerPrefs.SetInt(shopItemsSO[btnNo].levelSaveName, PlayerPrefs.GetInt(shopItemsSO[btnNo].levelSaveName, 0) + 1);
                break;

            case ShopItemSO.ItemType.RacketSize:
                PlayerPrefs.SetInt(shopItemsSO[btnNo].levelSaveName, PlayerPrefs.GetInt(shopItemsSO[btnNo].levelSaveName, 0) + 1);
                break;

            case ShopItemSO.ItemType.RacketSpeed:
                PlayerPrefs.SetInt(shopItemsSO[btnNo].levelSaveName, PlayerPrefs.GetInt(shopItemsSO[btnNo].levelSaveName, 0) + 1);
                break;
        }
        LoadCostAndButton(btnNo);
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

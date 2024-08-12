using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIGift : MonoBehaviour
{
    [SerializeField] private int[] coinsAmount;
    [SerializeField] private Color readyGift;
    [SerializeField] private Color closedGift;
    private TimeSpan giftInterval = TimeSpan.FromHours(8);
    private DateTime lastGiftTime;

    private TextMeshProUGUI timerText;
    [SerializeField] private GameObject coinsAquiredWindow;
    private TextMeshProUGUI coinsAquiredText;
    private Animator giftAnim;

    private Button openButton;
    private Image giftImage;
    private int nextGiftCoins;
    private bool IsReady;
    private void Awake()
    {
        openButton = GetComponent<Button>();
        timerText = transform.Find("timerText").GetComponent<TextMeshProUGUI>();
        giftImage = GetComponent<Image>();
        giftAnim = GetComponent<Animator>();

        giftImage.color = closedGift;        

        coinsAquiredWindow.transform.Find("panel").Find("closeBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            MusicSoundManager.Instance.PlayUI(GameAssets.Instance.closeButton);
            CloseCoinsAquiredWindow();
        });

        coinsAquiredText = coinsAquiredWindow.transform.Find("panel").Find("txt").GetComponent<TextMeshProUGUI>();

        openButton.onClick.AddListener(() =>
        {
            MusicSoundManager.Instance.PlayUI(GameAssets.Instance.itemBought);
            CollectGift();
        });
    }
    void Start()
    {
        coinsAquiredWindow.SetActive(false);
        // Loading last time of the gift
        if (PlayerPrefs.HasKey(PlayerPrefsVariables.Vars.LastGiftTime.ToString()))
        {
            lastGiftTime = DateTime.Parse(PlayerPrefs.GetString(PlayerPrefsVariables.Vars.LastGiftTime.ToString()));
        }
        else
        {
            lastGiftTime = DateTime.MinValue;
        }

        nextGiftCoins = coinsAmount[UnityEngine.Random.Range(0, coinsAmount.Length)];
        CheckGiftAvailability();
        StartCoroutine(UpdateTimerCoroutine());
        IsReady = IsGiftAvailable();
    }

    void CheckGiftAvailability()
    {
        if (IsGiftAvailable())
        {
            Debug.Log("Gift is available!");
        }
        else
        {
            Debug.Log("Gift is not available yet.");
        }
    }

    bool IsGiftAvailable()
    {
        bool timesUp = DateTime.Now - lastGiftTime >= giftInterval;
        bool coinsOut = TotalCoinsManager.Instance.GetCoinsAmount() < 15;
        if(coinsOut)
        {
            nextGiftCoins = 30;
        }
        return coinsOut || timesUp;
    }

    public void CollectGift()
    {
        if (IsGiftAvailable())
        {
            // Logic of gift collect
            Debug.Log("Gift collected!");
            TotalCoinsManager.Instance.AddCoins(nextGiftCoins);

            coinsAquiredText.SetText(nextGiftCoins.ToString());
            coinsAquiredWindow.SetActive(true);

            // Updating time of last gift collect and save it
            lastGiftTime = DateTime.Now;
            PlayerPrefs.SetString(PlayerPrefsVariables.Vars.LastGiftTime.ToString(), lastGiftTime.ToString());
            PlayerPrefs.Save();

            // Update gift availability
            CheckGiftAvailability();

            nextGiftCoins = coinsAmount[UnityEngine.Random.Range(0, coinsAmount.Length)];

            UpdateTimer();
        }
        else
        {
            Debug.Log("Gift is not available yet.");
        }
    }

    IEnumerator UpdateTimerCoroutine()
    {
        while (true)
        {
            UpdateTimer();
            yield return new WaitForSeconds(1); // Updating timer every second
        }
    }

    void UpdateTimer()
    {
        if (IsGiftAvailable())
        {
            if (openButton.IsInteractable() == false)
            {
                openButton.interactable = true;
            }

            if (giftImage.color == closedGift)
            {
                giftImage.color = readyGift;
            }
            timerText.SetText("OPEN!");
            if (IsReady == false)
            {
                IsReady = true;
                giftAnim.SetBool("IsReady", IsReady);
            }
        }
        else
        {
            TimeSpan timeRemaining = giftInterval - (DateTime.Now - lastGiftTime);
            timerText.SetText(string.Format("{0:D2}:{1:D2}:{2:D2}",
                                            timeRemaining.Hours,
                                            timeRemaining.Minutes,
                                            timeRemaining.Seconds));


            if (openButton.IsInteractable() == true)
            {
                openButton.interactable = false;
            }

            if (giftImage.color == readyGift)
            {
                giftImage.color = closedGift;
            }

            if (IsReady == true)
            {
                IsReady = false;
                giftAnim.SetBool("IsReady", IsReady);
            }
        }
    }
    public void CloseCoinsAquiredWindow()
    {
        coinsAquiredWindow.SetActive(false);
    }
}

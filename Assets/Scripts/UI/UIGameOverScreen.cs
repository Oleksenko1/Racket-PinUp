using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameOverScreen : MonoBehaviour
{
    private TextMeshProUGUI moneyEarnedText;
    private TextMeshProUGUI highestWinText;
    private int highestWin;
    private void Awake()
    {
        moneyEarnedText = transform.Find("panel").Find("moneyEarnedText").GetComponent<TextMeshProUGUI>();
        highestWinText = transform.Find("panel").Find("bestWin").Find("amountText").GetComponent<TextMeshProUGUI>();

        transform.Find("panel").Find("restartBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });

        transform.Find("panel").Find("homeBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        highestWin = PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.HighestWin.ToString(), 0);
    }
    private void Start()
    {
        GameOverManager.Instance.OnGameOver += ShowGameOverScreen;
        gameObject.SetActive(false);
    }
    private void ShowGameOverScreen()
    {
        gameObject.SetActive(true);

        // Shows coins gained amount
        int earnedCoins = TotalCoinsManager.Instance.GetCoinsEarned();
        moneyEarnedText.SetText(earnedCoins.ToString());

        // Shows highest score
        if(earnedCoins > highestWin)
        {
            highestWin = earnedCoins;
            PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.HighestWin.ToString(), highestWin);
        }
        highestWinText.SetText(highestWin.ToString());
    }
}

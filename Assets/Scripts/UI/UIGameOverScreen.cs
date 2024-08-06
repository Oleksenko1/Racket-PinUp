using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameOverScreen : MonoBehaviour
{
    [SerializeField] private Color winColor;
    private Transform winLabel;
    private Transform bestScoreText;
    private TextMeshProUGUI highestWinText;
    private int highestWin;
    private int earnedCoins;

    private Animator winLabelAnim;
    private TextMeshProUGUI winLabelTxt;
    private void Awake()
    {
        

        Transform panel = transform.Find("panel");

        bestScoreText = panel.Find("bestWin");

        winLabel = transform.Find("winLabel");
        winLabelAnim = winLabel.GetComponent<Animator>();
        winLabelTxt = winLabel.transform.Find("txt").GetComponent<TextMeshProUGUI>();

        highestWinText = bestScoreText.Find("amountText").GetComponent<TextMeshProUGUI>();

        panel.Find("restartBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });

        panel.Find("homeBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        highestWin = PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.HighestWin.ToString(), 0);

    }
    private void Start()
    {
        bestScoreText.gameObject.SetActive(false);
        GameOverManager.Instance.OnGameOver += DelayedShowGameScreen;
        gameObject.SetActive(false);

        UIMoneyEarnedCounter.Instance.OnCountingEnd += Counter_OnCountingEnd;
    }

    private void Counter_OnCountingEnd()
    {
        // Shows highest score
        highestWinText.SetText(highestWin.ToString()); 
        bestScoreText.gameObject.SetActive(true);
    }

    private void DelayedShowGameScreen()
    {
        Invoke(nameof(ShowGameOverScreen), 0.5f);
    }
    private void ShowGameOverScreen()
    {
        gameObject.SetActive(true);

        // Shows coins gained amount
        earnedCoins = TotalCoinsManager.Instance.GetCoinsEarned();
        UIMoneyEarnedCounter.Instance.StartCounting(earnedCoins);

        // If high score is beated - sets new high score
        if (earnedCoins > highestWin)
        {
            highestWin = earnedCoins;
            PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.HighestWin.ToString(), highestWin);
        }

        // If player won or lost
        if (UIMoneyEarnedCounter.Instance.GetCoinsForWin() <= earnedCoins)
        {
            winLabelTxt.SetText("WIN");
            winLabelTxt.color = winColor;
            winLabelAnim.Play("Win");
            MusicSoundManager.Instance.PlayMusic(GameAssets.Instance.victoryMusic);
        }
        else
        {
            winLabelTxt.SetText("LOSE");
            winLabelAnim.Play("Lose");
            MusicSoundManager.Instance.PlayMusic(GameAssets.Instance.looseMusic);
        }

        MusicSoundManager.Instance.FadeInMusic();
    }
}

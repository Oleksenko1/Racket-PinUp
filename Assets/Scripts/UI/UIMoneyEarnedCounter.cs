using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIMoneyEarnedCounter : MonoBehaviour
{
    public static UIMoneyEarnedCounter Instance { get; private set; }

    [SerializeField] private int coinsForWin = 60;
    [SerializeField] private float preferedCountSpeed = 3f;
    private TextMeshProUGUI scoreTxt;
    public event Action OnCountingEnd;

    private bool isCounting = false;
    private float currentScore = 0;
    private float targetScore;
    private AudioSource coinsAS;

    private ParticleSystem coinPS;
    private float particleAmount;

    private float actualSpeed;

    private void Awake()
    {
        Instance = this;

        scoreTxt = GetComponent<TextMeshProUGUI>();
        coinsAS = GetComponent<AudioSource>();
        coinPS = GetComponentInChildren<ParticleSystem>();

        coinsAS.clip = GameAssets.Instance.coinPayoutSFX;
    }
    public void StartCounting(int targetScore)
    {
        this.targetScore = targetScore;

        if (targetScore >= coinsForWin) // Plays winning animation and sounds
        {
            particleAmount = Mathf.Lerp(5, 30, 1f - (float)coinsForWin / targetScore); // Sets amount of particles depending on the coins gained
            ParticleSystem.EmissionModule emission = coinPS.emission;
            emission.rateOverTime = particleAmount;

            coinsAS.Play();

            coinPS.Play();
        }

        actualSpeed = targetScore / preferedCountSpeed;
        if (actualSpeed < 10) actualSpeed = 10;

        isCounting = true;
        UpdateText();
    }
    private void Update()
    {
        if (isCounting)
        {

            currentScore = Mathf.MoveTowards(currentScore, targetScore, actualSpeed * Time.deltaTime);
            UpdateText();

            if (currentScore == targetScore)
            {
                OnCountingEnd?.Invoke();
                coinsAS.Stop();
                isCounting = false;
                if (coinPS.isPlaying)
                {
                    coinPS.Stop();
                }
            }
        }
    }
    private void UpdateText()
    {
        scoreTxt.SetText(currentScore.ToString("0"));
    }
    public int GetCoinsForWin()
    {
        return coinsForWin;
    }
}

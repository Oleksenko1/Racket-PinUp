using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScoreText : MonoBehaviour
{
    public static UIScoreText Instance { get; private set; }

    private TextMeshProUGUI text;
    private void Awake()
    {
        Instance = this;
        text = transform.Find("scoreText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        TotalCoinsManager.Instance.OnCoinsChanged += UpdateText;

        UpdateText(TotalCoinsManager.Instance.GetCoinsAmount());
    }
    private void UpdateText(int value)
    {
        text.SetText(value.ToString());
    }
}

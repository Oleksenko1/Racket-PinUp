using UnityEngine;
using UnityEngine.Events;

public class TotalCoinsManager : MonoBehaviour
{
    public static TotalCoinsManager Instance { get; private set; }
    public UnityAction<int> OnCoinsChanged;

    private int totalCoins;
    private int coinsEarned = 0; // Money earned this round
    private void Awake()
    {
        Instance = this;

        totalCoins = PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.TotalCoins.ToString(), 20);
    }

    public void AddCoins(int value)
    {
        totalCoins += value;
        coinsEarned += value;
        PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.TotalCoins.ToString(), totalCoins);
        OnCoinsChanged?.Invoke(totalCoins);
    }
    public bool DiscardCoins(int value)
    {
        if(totalCoins >= value)
        {
            totalCoins -= value;
            PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.TotalCoins.ToString(), totalCoins);
            OnCoinsChanged?.Invoke(totalCoins);
            return true;
        }
        return false;
    }
    public int GetCoinsAmount()
    {
        return totalCoins;
    }
    public int GetCoinsEarned() // Coins earned this round
    {
        return coinsEarned;
    }
}

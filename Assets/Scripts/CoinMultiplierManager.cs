using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMultiplierManager : MonoBehaviour
{
    public static CoinMultiplierManager Instance { get; private set; }

    private int totalMult;

    private void Awake()
    {
        Instance = this;
    }
    public int GetMultiplier()
    {
        return totalMult;
    }
    public void IncreaseMultiplier(int increaseAmount)
    {
        totalMult += increaseAmount;
    }
    public void DecreaseMultiplier(int decreaseAmount)
    {
        totalMult -= decreaseAmount;
    }
}

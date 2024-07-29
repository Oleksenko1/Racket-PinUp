using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringWall : MonoBehaviour
{
    private TotalCoinsManager totalCoinsManager;
    private CoinMultiplierManager multiplier;
    private void Start()
    {
        totalCoinsManager = TotalCoinsManager.Instance;
        multiplier = CoinMultiplierManager.Instance;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Ball"))
        {
            int value = collision.transform.GetComponent<BallScript>().GetValue();
            value *= multiplier.GetMultiplier();

            totalCoinsManager.AddCoins(value);

            CoinPopup.Create(collision.transform.position, value);
        }
    }
}

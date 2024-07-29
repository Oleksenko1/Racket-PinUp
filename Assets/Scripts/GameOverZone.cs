using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ball"))
        {
            collision.GetComponent<BallScript>().Die();

            GameOverManager.Instance.BallDied();
        }
    }
}

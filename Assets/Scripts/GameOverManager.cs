using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    private int currentBalls = 0;
    private bool isGameOver = false;

    public UnityAction OnGameOver;
    private void Awake()
    {
        Instance = this;

        UILaunchBall.OnBallLaunched += BallLaunched;
    }
    private void BallLaunched()
    {
        currentBalls++;
    }
    public void BallDied()
    {
        currentBalls--;
        if(currentBalls <= 0)
        {
            isGameOver = true;
            OnGameOver?.Invoke();
            Debug.Log("Game over");
        }
    } 
    public bool GetGameOver()
    {
        return isGameOver;
    }
}

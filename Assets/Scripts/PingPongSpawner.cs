using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongSpawner : MonoBehaviour
{
    [SerializeField] private Transform ballPrefab;
    [SerializeField] private Vector2 boundries = new Vector2(2.5f, 5.5f);
    [SerializeField] private float spawnDelay;

    private float timerMax = 0;
    private float distanceFromCamera = 20f;

    private void Update()
    {
        timerMax -= Time.deltaTime;
        if(timerMax <= 0)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-boundries.x, boundries.x), Random.Range(-boundries.y, boundries.y), distanceFromCamera);
            if(spawnPosition.y > 0 || spawnPosition.y < -2)
            {
                timerMax += spawnDelay;
                Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}

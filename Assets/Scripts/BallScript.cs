using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] private float ballSpeed;
    [SerializeField] private int value = 1;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        LaunchBall();

        CoinMultiplierManager.Instance.IncreaseMultiplier(1);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.velocity = rb.velocity.normalized * ballSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionTag = collision.gameObject.tag;

        switch (collisionTag)
        {
            case "Racket":
                Vector2 racketPosition = collision.transform.position;
                Vector2 contactPoint = collision.GetContact(0).point;

                // Calculate offset from the center of the racket
                float offsetX = contactPoint.x - racketPosition.x;
                float racketWidth = collision.collider.bounds.size.x;

                // Calculate normalized hit position (-1 to 1)
                float correctionAmount = 1 - RacketController.Instance.GetCorrectionAmount(); // 0 - every shot is perfect, 1 - shots are not affected by correction
                float normalizedHitPosition = offsetX * correctionAmount / (racketWidth / 2);

                // Calculate new direction
                Vector2 newDirection = new Vector2(normalizedHitPosition, 1).normalized;

                // Reflect the ball's velocity based on the new direction
                rb.velocity = newDirection * ballSpeed;

                MusicSoundManager.Instance.PlaySFX(GameAssets.Instance.racketHit);

                Instantiate(GameAssets.Instance.hitParticle, contactPoint, collision.transform.rotation);

                break;

            case "ScorringWall":
                MusicSoundManager.Instance.PlaySFX(GameAssets.Instance.ballBounce);
                break;

        }
    }
    private void LaunchBall()
    {
        float offsetAmount = Random.Range(-0.03f, 0.03f);
        Vector2 launchOffset = new Vector2(0 + offsetAmount, 1 - offsetAmount).normalized;
        rb.velocity = launchOffset * ballSpeed;
    }
    public void Die()
    {
        Debug.Log("Ball died");
        CoinMultiplierManager.Instance.DecreaseMultiplier(1);
        MusicSoundManager.Instance.PlaySFX(GameAssets.Instance.ballDie);
        Destroy(gameObject);
    }
    public int GetValue()
    {
        return value;
    }
}

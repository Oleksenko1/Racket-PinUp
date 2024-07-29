using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILaunchBall : MonoBehaviour
{
    [SerializeField] private float offsetY = 1f;
    [SerializeField] private int launchCost = 2;
    [SerializeField] private float reloadTime = 2f;

    public static UnityAction OnBallLaunched;
    private float reloadTimeMax;
    private RacketController racket;
    private Image reloadImage;

    private void Start()
    {
        racket = RacketController.Instance;
        reloadImage = transform.Find("reloadBtn").GetComponent<Image>();

        reloadTimeMax = reloadTime;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (reloadTimeMax >= reloadTime && GameOverManager.Instance.GetGameOver() == false) // If reloading ended AND if game is not over
            {
                if (TotalCoinsManager.Instance.DiscardCoins(launchCost)) // If there are enough money - it's discards them
                {
                    Transform basicBall = GameAssets.Instance.basicBall;

                    Vector3 racketPosition = racket.GetRacketPosition();
                    racketPosition.y += offsetY;

                    Instantiate(basicBall, racketPosition, Quaternion.identity);

                    OnBallLaunched?.Invoke();

                    reloadTimeMax = 0; // Starts recharging;

                    MusicSoundManager.Instance.PlayUI(GameAssets.Instance.launchBall);
                }
            }
            else
            {
                Debug.Log("Not enough money or button is rechrarging");

                MusicSoundManager.Instance.PlayUI(GameAssets.Instance.decline);
            }
        });
    }

    private void Update()
    {
        if(reloadTimeMax <= reloadTime)
        {
            reloadTimeMax += Time.deltaTime;
            reloadImage.fillAmount = 1 - reloadTimeMax / reloadTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameAssets>("GameAssets");
            }
            return instance;
        }
    }

    [SerializeField] public Transform basicBall;
    [SerializeField] public Transform coinPopup;
    [SerializeField] public Transform hitParticle;

    [Header("Musics")]
    [SerializeField] public AudioClip menuMusic;
    [SerializeField] public AudioClip playingMusic;
    [SerializeField] public AudioClip victoryMusic;
    [SerializeField] public AudioClip looseMusic;

    [Header("SFX")]
    [SerializeField] public AudioClip ballBounce;
    [SerializeField] public AudioClip coinCollected;
    [SerializeField] public AudioClip racketHit;
    [SerializeField] public AudioClip ballDie;

    [Header("UI")] 
    [SerializeField] public AudioClip basicButton;
    [SerializeField] public AudioClip closeButton;
    [SerializeField] public AudioClip playBtn;
    [SerializeField] public AudioClip launchBall;
    [SerializeField] public AudioClip decline;
    [SerializeField] public AudioClip itemBought;
    [SerializeField] public AudioClip tabSelected;
    [SerializeField] public AudioClip coinPayoutSFX;

    [Header("AudioMixers")]
    [SerializeField] public AudioMixer musicMixer;
    [SerializeField] public AudioMixer sfxMixer;
    [SerializeField] public AudioMixer uiMixer;

    [Header("UpgradeStats")]
    [SerializeField] public ShopItemSO racketAccuracy;
    [SerializeField] public ShopItemSO racketSpeed;
    [SerializeField] public ShopItemSO racketSize;
    [SerializeField] public ShopItemSO wallsUnlocked;

    [Header("Shop")]
    [SerializeField] public GameObject upgradeItemTemplate;
    [SerializeField] public GameObject backgroundItemTemplate;
    [SerializeField] public GameObject musicItemTemplate;

    [Space(15)]
    [SerializeField] public ShopItemListSO upgradeItemsList;
    [SerializeField] public ShopItemListSO backgroundItemsList;
    [SerializeField] public ShopItemListSO musicItemsList;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;
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

    [Header("Musics")]
    [SerializeField] public AudioClip menuMusic;
    [SerializeField] public AudioClip playingMusic;

    [Header("SFX")]
    [SerializeField] public AudioClip ballBounce;
    [SerializeField] public AudioClip coinCollected;
    [SerializeField] public AudioClip racketHit;

    [Header("UI")] 
    [SerializeField] public AudioClip basicButton;
    [SerializeField] public AudioClip closeButton;
    [SerializeField] public AudioClip playBtn;
    [SerializeField] public AudioClip launchBall;
    [SerializeField] public AudioClip decline;
    [SerializeField] public AudioClip itemBought;

    [Header("AudioMixers")]
    [SerializeField] public AudioMixer musicMixer;
    [SerializeField] public AudioMixer sfxMixer;
    [SerializeField] public AudioMixer uiMixer;

    [Header("RacketStats")]
    [SerializeField] public ShopItemSO racketAccuracy;
    [SerializeField] public ShopItemSO racketSpeed;
    [SerializeField] public ShopItemSO racketSize;
}

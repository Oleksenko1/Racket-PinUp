using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicSoundManager : MonoBehaviour
{
    public static MusicSoundManager Instance;

    public enum volumeType
    { 
        music,
        sfx,
        ui
    }


    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource uiSource;

    [SerializeField] private AudioMixerSnapshot silentMusicSnapshot;
    [SerializeField] private AudioMixerSnapshot fullMusicSnapshot;

    private float musicVolume;
    private float sfxVolume;
    private float uiVolume;

    private AudioClip currentMusic;

    private void Awake()
    {
        Instance = this;

        musicSource = transform.Find("musicSource").GetComponent<AudioSource>();
        sfxSource = transform.Find("sfxSource").GetComponent<AudioSource>();
        uiSource = transform.Find("uiSource").GetComponent<AudioSource>();
    }
    private void Start()
    {
        LoadMusic();

        InitializeVolume();

        //PlayMusic(GameAssets.Instance.basicMusic);
    }
    private void InitializeVolume()
    {
        SetVolume(volumeType.music, PlayerPrefs.GetFloat(PlayerPrefsVariables.Vars.MusicVolumeFloat.ToString(), 0.8f));
        SetVolume(volumeType.sfx, PlayerPrefs.GetFloat(PlayerPrefsVariables.Vars.SoundsVolumeFloat.ToString(), 0.8f));
        SetVolume(volumeType.ui, PlayerPrefs.GetFloat(PlayerPrefsVariables.Vars.UIVolumeFloat.ToString(), 0.8f));

        ToggleMusic(PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.musicPlayIsOn.ToString()) == 1);
    }
    public void PlayMusic(AudioClip audioClip)
    {
        if (PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.musicPlayIsOn.ToString()) == 1)
        {
            musicSource.clip = audioClip;
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }
    }
    public void LoadMusic()
    {
        switch (GameSceneManager.GetCurrentScene())
        {
            case GameSceneManager.Scene.GameScene:
                currentMusic = GameAssets.Instance.musicItemsList.list[PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.SelectedMusic.ToString(), 0)].music;
                break;

            case GameSceneManager.Scene.MainMenuScene:
                currentMusic = GameAssets.Instance.menuMusic;
                break;
        }

    }
    public void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }
    public void PlayUI(AudioClip audioClip)
    {
        uiSource.PlayOneShot(audioClip);
    }

    public void SetVolume(volumeType type, float value)
    {
        switch (type)
        {
            case volumeType.music:
                musicVolume = Mathf.Lerp(-80, 5, value);
                GameAssets.Instance.musicMixer.SetFloat("Volume", musicVolume);
                PlayerPrefs.SetFloat(PlayerPrefsVariables.Vars.MusicVolumeFloat.ToString(), value);
                break;

            case volumeType.sfx:
                sfxVolume = Mathf.Lerp(-80, 5, value);
                GameAssets.Instance.sfxMixer.SetFloat("Volume", sfxVolume);
                PlayerPrefs.SetFloat(PlayerPrefsVariables.Vars.SoundsVolumeFloat.ToString(), value);
                break;

            case volumeType.ui:
                uiVolume = Mathf.Lerp(-80, 5, value);
                GameAssets.Instance.uiMixer.SetFloat("Volume", uiVolume);
                PlayerPrefs.SetFloat(PlayerPrefsVariables.Vars.UIVolumeFloat.ToString(), value);
                break;
        }  
    }
    public AudioClip CurrentMusic()
    {
        return musicSource.clip;
    }
    public void ToggleMusic(bool toggleMusic)
    {
        PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.musicPlayIsOn.ToString(), toggleMusic ? 1 : 0);

        PlayMusic(currentMusic);
    }
    public void FadeOutMusic()
    {
        silentMusicSnapshot.TransitionTo(0.5f);
    }
    public void FadeInMusic()
    {
        fullMusicSnapshot.TransitionTo(0.5f);
    }
}

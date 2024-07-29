using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsPanel : MonoBehaviour
{
    private void Awake()
    {
        InitializeSettingsButtons();
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void InitializeSettingsButtons()
    {
        Transform panel = transform.Find("panel");
        // Music volume slider
        Slider musicSlider = panel.Find("musicSlider").GetComponent<Slider>();
        musicSlider.value = PlayerPrefs.GetFloat(PlayerPrefsVariables.Vars.MusicVolumeFloat.ToString(), 0.8f);
        musicSlider.onValueChanged.AddListener((value) =>
        {
            MusicSoundManager.Instance.SetVolume(MusicSoundManager.volumeType.music, value);
        });

        // Music toggle checkbox
        Toggle musicToggle = panel.Find("musicPlayToggle").GetComponent<Toggle>();
        musicToggle.isOn = PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.musicPlayIsOn.ToString(), 0) == 1 ? true : false;
        musicToggle.onValueChanged.AddListener((isOn) =>
        {
            MusicSoundManager.Instance.PlayUI(GameAssets.Instance.basicButton);

            MusicSoundManager.Instance.ToggleMusic(isOn);
        });

        // Sounds volume slider
        Slider soundsSlider = panel.Find("soundsSlider").GetComponent<Slider>();
        soundsSlider.value = PlayerPrefs.GetFloat(PlayerPrefsVariables.Vars.SoundsVolumeFloat.ToString(), 0.8f);
        soundsSlider.onValueChanged.AddListener((value) =>
        {
            MusicSoundManager.Instance.SetVolume(MusicSoundManager.volumeType.sfx, value);
        });

        // UI volume slider
        Slider UISlider = panel.Find("UISlider").GetComponent<Slider>();
        UISlider.value = PlayerPrefs.GetFloat(PlayerPrefsVariables.Vars.UIVolumeFloat.ToString(), 0.8f);
        UISlider.onValueChanged.AddListener((value) =>
        {
            MusicSoundManager.Instance.SetVolume(MusicSoundManager.volumeType.ui, value);
        });

        // Close button
        Button closeBtn = panel.Find("closeBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(() =>
        {
            MusicSoundManager.Instance.PlayUI(GameAssets.Instance.closeButton);
            gameObject.SetActive(false);
            Time.timeScale = 1;
        });
    }

    public void OpenSettings()
    {
        MusicSoundManager.Instance.PlayUI(GameAssets.Instance.basicButton);
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}

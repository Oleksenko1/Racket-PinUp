using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    private Transform settingsPanel;

    private void Awake()
    {
        settingsPanel = transform.Find("SettingsPanel");

        InitializeMenuButtons();
    }
    private void InitializeMenuButtons()
    {
        Transform menuButtons = transform.Find("MainMenuButtons");

        menuButtons.Find("playBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
            MusicSoundManager.Instance.PlayUI(GameAssets.Instance.playBtn);
        });
        menuButtons.Find("settingsBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            settingsPanel.GetComponent<UISettingsPanel>().OpenSettings();
        });
        menuButtons.Find("exitBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            MusicSoundManager.Instance.PlayUI(GameAssets.Instance.closeButton);
            Application.Quit();
        });
    }
}

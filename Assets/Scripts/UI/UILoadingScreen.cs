using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingScreen : MonoBehaviour
{

    private void Start()
    {
        Invoke(nameof(LoadMenu), 5.5f);
    }

    private void LoadMenu()
    {
        GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
    }
}

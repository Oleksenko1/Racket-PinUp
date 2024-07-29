using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class GameSceneManager
{
    public enum Scene
    {
        GameScene,
        MainMenuScene
    }
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
    public static Scene GetCurrentScene()
    {
        string currentSceneString = SceneManager.GetActiveScene().name;

        Scene currentScene = (Scene)Enum.Parse(typeof(Scene), currentSceneString, true);

        return currentScene;
    }
}

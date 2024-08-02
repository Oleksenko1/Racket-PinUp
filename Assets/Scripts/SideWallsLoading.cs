using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWallsLoading : MonoBehaviour
{
    private int wallsUnlcoked;
    private void Awake()
    {
        wallsUnlcoked = PlayerPrefs.GetInt(GameAssets.Instance.wallsUnlocked.levelSaveName, 0);

        for (int i = 1; i <= wallsUnlcoked; i++)
        {
            transform.Find("wall" + i).gameObject.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScript : MonoBehaviour
{
    private Image bgImage;
    private void Awake()
    {
        int selectedIndex = PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.SelectedBackGround.ToString(), 0);
        bgImage = transform.Find("image").GetComponent<Image>();

        bgImage.sprite = GameAssets.Instance.backgroundItemsList.list[selectedIndex].shopIcon;
    }
}

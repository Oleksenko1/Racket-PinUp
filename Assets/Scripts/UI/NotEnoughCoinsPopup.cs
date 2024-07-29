using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughCoinsPopup : MonoBehaviour
{
    public static NotEnoughCoinsPopup Instance;
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void OpenMessageBox()
    {
        gameObject.SetActive(true);
    }
    public void CloseMessageBox()
    {
        MusicSoundManager.Instance.PlayUI(GameAssets.Instance.closeButton);
        gameObject.SetActive(false);
    }
}

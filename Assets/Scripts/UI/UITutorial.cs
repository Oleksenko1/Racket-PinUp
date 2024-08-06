using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    private Transform panel;
    private Button leftBtn;
    private Button rightBtn;
    private Button closeBtn;

    private List<GameObject> pagesList = new List<GameObject>();

    private int currentPage = 0;
    private void Awake()
    {
        InstantiateTutorial();
    }
    private void InstantiateTutorial()
    {
        // If tutorial is enabled
        if (PlayerPrefs.GetInt(PlayerPrefsVariables.Vars.ShowTutorial.ToString(), 1) == 1)
        {
            panel = transform.Find("panel");
            Transform pagesTransform = panel.Find("Pages");
            // Adds every tutorial page to the list and turns them off
            foreach (Transform page in pagesTransform)
            {
                pagesList.Add(page.gameObject);
                page.gameObject.SetActive(false);
            }

            leftBtn = panel.Find("leftBtn").GetComponent<Button>();
            leftBtn.onClick.AddListener(() =>
            {
                ChangePage(currentPage - 1);
                MusicSoundManager.Instance.PlayUI(GameAssets.Instance.basicButton);
            });

            rightBtn = panel.Find("rightBtn").GetComponent<Button>();
            rightBtn.onClick.AddListener(() =>
            {
                ChangePage(currentPage + 1);
                MusicSoundManager.Instance.PlayUI(GameAssets.Instance.basicButton);
            });

            closeBtn = panel.Find("closeBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(() =>
            {
                MusicSoundManager.Instance.PlayUI(GameAssets.Instance.closeButton);
                CloseTutorial();
            });

            panel.Find("dontShowToggle").GetComponent<Toggle>().onValueChanged.AddListener((bool b) =>
            {
                int hideTutorial = b ? 0 : 1;
                PlayerPrefs.SetInt(PlayerPrefsVariables.Vars.ShowTutorial.ToString(), hideTutorial);
            });

            ChangePage(0);
        }
        else
        {
            CloseTutorial();
        }
    }
    private void ChangePage(int pageIndex)
    {
        // Turns off every page
        for (int i = 0; i < pagesList.Count; i++)
        {
            pagesList[i].SetActive(false);
        }
        pagesList[pageIndex].SetActive(true);
        currentPage = pageIndex;

        closeBtn.gameObject.SetActive(false);
        leftBtn.interactable = true;
        rightBtn.interactable = true;

        if (currentPage == 0) // If currently on the first page
        {
            leftBtn.interactable = false;
        }
        if(currentPage == pagesList.Count - 1) // If on the last page currently
        {
            rightBtn.interactable = false;
            closeBtn.gameObject.SetActive(true);
        }
    }
    private void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
}

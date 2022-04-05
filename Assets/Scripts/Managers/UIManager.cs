using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Panels
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;
    #endregion
    #region SINGLETON
    private static UIManager instance;
    public static UIManager Instance => instance ??= FindObjectOfType<UIManager>();
    #endregion

    public void GameStart() 
    {
        startPanel.SetActive(false);
        hudPanel.SetActive(true);
    }
    public void GameEnd(bool won) 
    {
        hudPanel.SetActive(false);
        if (won)
        {
            winPanel.SetActive(true);
        }
        else
        {
            failPanel.SetActive(true);
        }
    }
    public void NextLevel() 
    {
        winPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void RestartLevel() 
    {
        failPanel.SetActive(false);
        startPanel.SetActive(true);
    }
}

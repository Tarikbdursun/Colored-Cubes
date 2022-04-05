using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    private static GameManager instance;
    public static GameManager Instance => instance ??= FindObjectOfType<GameManager>();
    #endregion
    public bool isGameStarted;
    [SerializeField] private LevelList levelList;
    void Start()
    {
        LevelManager.SetLevelManager(levelList);
    }

    public void GameStart() 
    {
        isGameStarted = true;
        UIManager.Instance.GameStart();
    }
    public void GameEnd(bool won) 
    {
        isGameStarted = false;

        UIManager.Instance.GameEnd(won);
    }
    public void NextLevel() 
    {
        LevelManager.NextLevel();
        UIManager.Instance.NextLevel();
        Player.Instance.PlayerReset();
        CameraManager.Instance.CameraReset();
    }
    public void RestartLevel() 
    {
        LevelManager.RestartLevel(); 
        UIManager.Instance.RestartLevel();
        Player.Instance.PlayerReset();
        CameraManager.Instance.CameraReset();
    }
}

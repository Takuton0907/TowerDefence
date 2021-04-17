using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary> ゲーム全体の管理 </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    enum GAMESTATE
    {
        INIT,
        TITLE,
        SELECT,
        GAME,
    }
    GAMESTATE _gameState = GAMESTATE.INIT;

    Dictionary<StageData, int> stageResalts = new Dictionary<StageData, int>();

    public StageData stage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        switch (_gameState)
        {
            case GAMESTATE.INIT:
#if UNITY_EDITOR
                if (SceneManager.GetActiveScene().name == "TITLE")
                {
                    StartCoroutine(SoundManager.Instance.SetBgmAudio());
                    _gameState = GAMESTATE.TITLE;
                }
                else if (SceneManager.GetActiveScene().name == "SELECT")
                {
                    _gameState = GAMESTATE.SELECT;
                }
                else
                {
                    _gameState = GAMESTATE.GAME;
                }
#endif
                if (_gameState == GAMESTATE.INIT)
                {
                    StartCoroutine(SoundManager.Instance.SetBgmAudio());
                    _gameState = GAMESTATE.TITLE;
                }
                break;
            case GAMESTATE.TITLE:
                if (Input.GetMouseButtonDown(0))
                {
                    FadeManager.Instance.LoadScene("SELECT", 1);
                    _gameState = GAMESTATE.SELECT;
                }
                break;
            case GAMESTATE.SELECT:
                break;
            case GAMESTATE.GAME:
                break;
            default:
                break;
        }
    }

    public void SetClearValue(StageData key, int value)
    {
        Debug.Log(key);
        if (stageResalts.ContainsKey(key))
        {
            if (stageResalts[key] <= value)
            {
                stageResalts[key] = value;
            }
        }
        else
        {
            stageResalts.Add(key, value);
        }
    }

    public int GetResaltValue(StageData key)
    {
        if (stageResalts.ContainsKey(key))
        {
            return stageResalts[key];
        }
        else
        {
            return 0;
        }
    }
}
/// <summary> ステージのデータ </summary>
[Serializable]
public class StageData
{
   public GameObject stage;
   public TextAsset enemyData;
   public MapDataObject mapData;
}
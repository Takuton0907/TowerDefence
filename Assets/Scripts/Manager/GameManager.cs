using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    Dictionary<string, int> stageResalts = new Dictionary<string, int>();

    private void Update()
    {
        switch (_gameState)
        {
            case GAMESTATE.INIT:
#if UNITY_EDITOR
                if (SceneManager.GetActiveScene().name == "TITLE")
                {
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

    public void SetClearValue(string key, int value)
    {
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

    public int GetResaltValue(string key)
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
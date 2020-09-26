using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum GAMESTATE
    {
        INIT,
        TITLE,
        SELECT,
        GAME,
    }
    GAMESTATE _gameState = GAMESTATE.INIT;


    private void Update()
    {
        switch (_gameState)
        {
            case GAMESTATE.INIT:
                break;
            case GAMESTATE.TITLE:
                if (Input.GetMouseButtonDown(0))
                {
                    FadeManager.Instance.LoadScene("GameTest", 1);
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
}

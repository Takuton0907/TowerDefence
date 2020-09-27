using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField]
    GameObject _star;

    private void Start()
    {
        int count = GameManager.Instance.GetResaltValue(gameObject.name);
        Text text = GetComponentInChildren<Text>();
        for (int i = 0; i < count; i++)
        {
            Instantiate(_star, text.gameObject.transform);
        }
    }

    public void OcClickGameStart()
    {
        GameManager.Instance.nextGameStagePath = gameObject.name;
        FadeManager.Instance.LoadScene("GAME", 1.5f);
    }
}

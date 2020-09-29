using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField]
    GameObject _star;

    AudioSource audioSource;

    private void Start()
    {
        int count = GameManager.Instance.GetResaltValue(gameObject.name);
        Text text = GetComponentInChildren<Text>();
        for (int i = 0; i < count; i++)
        {
            Instantiate(_star, text.gameObject.transform);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void OcClickGameStart()
    {
        audioSource.Play();
        GameManager.Instance.nextGameStagePath = gameObject.name;
        StartCoroutine(CharaMove());
    }

    IEnumerator CharaMove()
    {
        Animator charaAnim = GameObject.Find("Chara").GetComponent<Animator>();

        charaAnim.SetBool("Start", true);

        while (!charaAnim.GetCurrentAnimatorStateInfo(0).IsName("SelectStart"))
        {
            yield return null;
        }

        while (charaAnim.GetCurrentAnimatorStateInfo(0).IsName("SelectStart"))
        {
            yield return null;      
        }

        charaAnim.enabled = false;
        FadeManager.Instance.LoadScene("GAME", 1.5f);
    }
}

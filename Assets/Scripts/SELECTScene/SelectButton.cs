using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 選択画面のボタンでの処理 </summary>
public class SelectButton : MonoBehaviour
{
    [SerializeField]
    GameObject _star;

    AudioSource audioSource;

    [SerializeField]
    StageData stageData = null;

    private void Start()
    {
        int count = GameManager.Instance.GetResaltValue(stageData);
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
        GameManager.Instance.stage = stageData;
        StartCoroutine(CharaMove());
    }
    /// <summary> キャラクターの移動 </summary>
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

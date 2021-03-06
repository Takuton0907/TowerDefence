﻿using UnityEngine;
using UnityEngine.UI;

/// <summary> ステージ選択画面の管理 </summary>
public class SelectSceneManager : MonoBehaviour
{
    [SerializeField]
    Button _selectButton;

    [SerializeField]
    Transform _parentTrans;

    AudioSource _bgmAudio;

    private void Awake()
    {
        _bgmAudio = GetComponent<AudioSource>();

        StartCoroutine(SoundManager.Instance.SetBgmAudio(_bgmAudio.clip));
    }
}
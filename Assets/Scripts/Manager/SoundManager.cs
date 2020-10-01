using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] AudioSource _buffEffectAudios;

    [SerializeField] AudioSource _deBuffEffectAudios;

    [SerializeField] AudioSource _BGMSudio;

    [SerializeField] AudioSource _titleSound;

    private float nomalPitch; 

    private new void Awake()
    {
        base.Awake();
        nomalPitch = _BGMSudio.pitch;
    }

    public void SetEffectAudio(AudioClip clip)
    {
        _buffEffectAudios.clip = clip;
        _buffEffectAudios.Play();
        _buffEffectAudios.loop = true;

    }

    public void SetDeeffectAudio(AudioClip clip)
    {
        _deBuffEffectAudios.clip = clip;
        _deBuffEffectAudios.Play();
        _deBuffEffectAudios.loop = true;
    }

    public IEnumerator SetBgmAudio(AudioClip clip)
    {
        if (_BGMSudio.volume != 0)
        {
            while (_BGMSudio.volume > 0)
            {
                _BGMSudio.volume -= 1 * Time.deltaTime;
                yield return null;
            }
        }

        _BGMSudio.clip = clip;

        _BGMSudio.Play();

        while (_BGMSudio.volume < 0.6f)
        {
            _BGMSudio.volume += 1 * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator SetBgmAudio()
    {
        if (_BGMSudio.volume != 0)
        {
            while (_BGMSudio.volume > 0)
            {
                _BGMSudio.volume -= 1 * Time.deltaTime;
                yield return null;
            }
        }

        _BGMSudio.clip = _titleSound.clip;

        _BGMSudio.Play();

        while (_BGMSudio.volume < 0.6f)
        {
            _BGMSudio.volume += 1 * Time.deltaTime;
            yield return null;
        }
    }

    public void StageFin()
    {
        _buffEffectAudios.clip = null;
        _deBuffEffectAudios.clip = null;
    }

    public void BGMUpPitch()
    {
        _BGMSudio.pitch = 1.3f;
    }

    public void BGMDownPitch()
    {
        _BGMSudio.pitch = nomalPitch;
    }
}

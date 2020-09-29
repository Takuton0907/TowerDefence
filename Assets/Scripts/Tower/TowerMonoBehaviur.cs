using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class TowerMonoBehaviur : MonoBehaviour
{
    [SerializeField] protected GameObject _UIObject;

    [SerializeField] protected AudioSource _actionAudio;

    public GameObject _buffAnimObj;

    protected Animator _animator;

    protected List<TowerAnimBase> _anims = new List<TowerAnimBase>();

    public Transform parentTransform;

    public float _area = 2;

    virtual public void Init()
    {
        _UIObject.SetActive(false);
        _animator = GetComponent<Animator>();
        _actionAudio = GetComponent<AudioSource>();
    }

    virtual public void OnClickOpenCanvas()
    {
        _UIObject.SetActive(true);
        LevelManager.Instance.CahraClick(gameObject);
    }
    public virtual void CloseCanvas() => _UIObject.SetActive(false);
    public virtual void OnClickRemoveTower()
    {
        LevelManager.Instance.OnClickRemoveChara(this);
        LevelManager.Instance.CameraReset();
        Destroy(gameObject);
    }
    public virtual void RemoveAnim(TowerAnimBase towerAnimBase) => _anims.Remove(towerAnimBase);
    public virtual void RemoveAnimAll() => _anims.RemoveAll((a) => _anims.Remove(a));
    public virtual void Action(float speed) { }
    public virtual void Action(List<EnemyCon> enemy, float speed) { }
    public virtual void SoundOFF()
    {
        _actionAudio.Stop();
    }
    public virtual void StopEffect()
    {
        RemoveAnimAll();
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }
}
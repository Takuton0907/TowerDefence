using System.Collections.Generic;
using UnityEngine;

/// <summary> タワーのBaseクラス </summary>
[RequireComponent(typeof(AudioSource))]
public abstract class TowerBase : MonoBehaviour
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
    /// <summary> タワーごとのUIを付ける </summary>
    virtual public void OnClickOpenCanvas()
    {
        _UIObject.SetActive(true);
        LevelManager.Instance.CahraClick(gameObject);
    }
    /// <summary> タワーごとのUIを消す </summary>
    public virtual void CloseCanvas() => _UIObject.SetActive(false);

    /// <summary> タワーの退却 </summary>
    public virtual void OnClickRemoveTower()
    {
        LevelManager.Instance.OnClickRemoveChara(this);
        LevelManager.Instance.CameraReset();
        Destroy(gameObject);
    }
    /// <summary> アニメーションの削除 </summary>
    public virtual void RemoveAnim(TowerAnimBase towerAnimBase) => _anims.Remove(towerAnimBase);
    /// <summary> アニメーションをすべて削除 </summary>
    public virtual void RemoveAnimAll() => _anims.RemoveAll((a) => _anims.Remove(a));
    /// <summary> タワーごとの行動 </summary>
    public virtual void Action(float speed) { }
    /// <summary> タワーごとの行動 </summary>
    public virtual void Action(List<EnemyCon> enemy, float speed) { }
    /// <summary> サウンドの停止 </summary>
    public virtual void SoundOFF()
    {
        _actionAudio.Stop();
    }
    /// <summary> エフェクトの停止 </summary>
    public virtual void StopEffect()
    {
        RemoveAnimAll();
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }
}
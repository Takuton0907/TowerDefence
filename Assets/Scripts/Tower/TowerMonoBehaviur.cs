using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerMonoBehaviur : MonoBehaviour
{
    [SerializeField] protected GameObject _UIObject;

    protected Animator _animator;

    protected List<TowerAnimBase> _anims = new List<TowerAnimBase>();

    public Transform parentTransform;

    public float _area = 2;

    virtual public void Init()
    {
        _UIObject.SetActive(false);
        _animator = GetComponent<Animator>();
    }

    virtual public void OnClickOpenCanvas()
    {
        _UIObject.SetActive(true);
    }
    public virtual void OnClickRemoveTower()
    {
        LevelManager.Instance.OnClickRemoveChara(this);
        Destroy(gameObject);
    }
    public virtual void RemoveAnim(TowerAnimBase towerAnimBase) => _anims.Remove(towerAnimBase); 
    public virtual void Action(float speed) { }
    public virtual void Action(List<EnemyCon> enemy, float speed) { }
}
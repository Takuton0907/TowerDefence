using System.Collections;
using UnityEngine;

/// <summary> タワーのアニメーションの管理 </summary>
public abstract class TowerAnimBase : MonoBehaviour
{
    [SerializeField] protected Vector3 _animTargetPosi;

    protected Vector3 _moveVector;

    protected TowerBase _parentTowerMono;

    protected bool _deth = false;

    [SerializeField] protected float _animSpeed;

    public virtual void SetAnimDirection(Vector3 value, TowerBase tower) 
    {
        _animTargetPosi = value;
        _moveVector = (value - transform.position).normalized;
        _parentTowerMono = tower;
    }

    public abstract void AnimUpdate(float speed);
    public virtual void Deth()
    {
        if (_deth == true)
        {
            StartCoroutine(Des());
            _deth = false;
        }
    }

    public virtual IEnumerator Des()
    {
        yield return null;
        _parentTowerMono.RemoveAnim(this);
        Destroy(gameObject);
    }
}

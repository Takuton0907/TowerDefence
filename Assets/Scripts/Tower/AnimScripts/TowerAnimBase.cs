using System.Collections;
using UnityEngine;

public abstract class TowerAnimBase : MonoBehaviour
{
    [SerializeField] protected Vector3 _animTargetPosi;

    protected Vector3 _moveVector;

    protected TowerMonoBehaviur _parentTowerMono;

    protected bool _deth = false;

    [SerializeField] protected float _animSpeed;

    public virtual void SetAnimDirection(Vector3 value, TowerMonoBehaviur towerMonoBehaviur) 
    {
        _animTargetPosi = value;
        _moveVector = (value - transform.position).normalized;
        _parentTowerMono = towerMonoBehaviur;
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

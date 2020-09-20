using UnityEngine;

public abstract class TowerAnimBase : MonoBehaviour
{
    [SerializeField] protected Vector3 _animTargetPosi;

    protected Vector3 _moveVector;

    [SerializeField] protected float _animSpeed;

    public virtual void SetAnimDirection(Vector3 value) 
    {
        _animTargetPosi = value;
        _moveVector = (value - transform.position).normalized;
    }

    public abstract void AnimUpdate(float speed);
}

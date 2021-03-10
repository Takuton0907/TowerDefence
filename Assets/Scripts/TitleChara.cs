using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleChara : MonoBehaviour
{
    [SerializeField] float _speed = 1;

    [SerializeField] float _downSpeed = 0.1f;

    public float _speedRate = 1;

    Animator _animator;

    public List<MAP_C_DATE> _root = new List<MAP_C_DATE>();

    private Vector3 _next;
    private Vector3 _moveDirection = new Vector3();
    public int count = 0;

    public void Awake()
    {
        _animator = GetComponent<Animator>();

        _root = TitleManager.Instance.Sarch(transform.position);
        _root.Reverse();
        _moveDirection = NextMove(transform.position);
    }

    private void Update()
    {
        if ((transform.position - _next).magnitude < 0.15f)
        {
            if (count == _root.Count)
            {
                Destroy(gameObject);
                return;
            }
            _moveDirection = NextMove(transform.position);
            _animator.SetFloat("Hori", _moveDirection.x);
            _animator.SetFloat("Var", _moveDirection.y);
        }
        transform.position += _moveDirection * _speed * _speedRate * Time.deltaTime;
    }

    //敵の次の動き
    private Vector3 NextMove(Vector3 nowPosi)
    {
        _next = _root[count].posi + new Vector3(0.5f, 0.5f, 0);//0.5ずらすのは位置の調整

        count++;
        Vector3 move = _next - nowPosi;
        move.z = 0;
        return move.normalized;
    }
}

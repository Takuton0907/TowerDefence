using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyCon : MonoBehaviour
{
    public int HP = 10;
    [SerializeField] float _speed = 1;

    public List<MAP_DATE> _root = new List<MAP_DATE>();

    private Vector3 _next;
    private Vector3 _moveDirection = new Vector3();
    private int count = 0;

    private void Awake()
    {
        _root = LevelManager.Instance.Sarch();
        Debug.Log(_root.Count);
        _root.Reverse();
        _moveDirection = NextMove(transform.position);
    }

    private void Update()
    {
        
        if ((transform.position - _next).magnitude < 0.01f)
        {
            if (count == _root.Count)
            {
                Destroy(gameObject);
                return;
            }
            _moveDirection = NextMove(transform.position);
        }
        transform.position += _moveDirection * _speed;
    }

    public void Damage(int damage)
    {
        HP = HP - damage;
        Debug.Log(HP);
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


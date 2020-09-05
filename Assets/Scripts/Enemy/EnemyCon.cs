using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyCon : MonoBehaviour
{
    public int HP = 10;
    [SerializeField] float _speed = 1;
    Rigidbody2D _rig2d;

    public List<MAP_DATE> _root = new List<MAP_DATE>();

    private Vector3 _next;
    private Vector3 _moveDirection = new Vector3();
    public int count = 0;

    private void Awake()
    {
        _root = LevelManager.Instance.Sarch();
        Debug.Log(_root.Count);
        _root.Reverse();
        _moveDirection = NextMove(transform.position);
        _rig2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
        if ((transform.position - _next).magnitude < 0.15f)
        {
            if (count == _root.Count)
            {
                EnemyManager.Instance.DestroyEnemy(gameObject.GetComponent<EnemyCon>());
                Destroy(gameObject);
                return;
            }
            _moveDirection = NextMove(transform.position);
        }
        //transform.position += _moveDirection * _speed;

        _rig2d.velocity = _moveDirection * _speed;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
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


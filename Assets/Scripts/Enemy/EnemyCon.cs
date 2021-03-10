using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyCon : MonoBehaviour
{
    public enum EnemySpeedState
    {
        NOMAL,
        UP,
        DOWN,
    }
    public EnemySpeedState _enemySpeed = EnemySpeedState.NOMAL;

    private bool _active = true;

    public int HP = 10;

    [SerializeField] float _speed = 1;

    [SerializeField] float _downSpeed = 0.1f;

    public float _speedRate = 1;

    [SerializeField] GameObject _animObj;

    [SerializeField] GameObject _desAnim;

    Animator _animator;

    public List<MAP_C_DATE> _root = new List<MAP_C_DATE>();

    private Vector3 _next;
    private Vector3 _moveDirection = new Vector3();
    public int count = 0;

    public void EnemyAwake()
    {
        _animator = GetComponent<Animator>();

        _root = LevelManager.Instance.Sarch(transform.position);
        Debug.Log(_root.Count);
        _root.Reverse();
        _moveDirection = NextMove(transform.position);
    }

    public void EnemyUpdate()
    {
        if (!_active) return;
        if ((transform.position - _next).magnitude < 0.15f)
        {
            if (count == _root.Count)
            {
                LevelManager.Instance.Damage();
                StartCoroutine(Des());
                //Destroy(gameObject);
                _active = false;
                return;
            }
            _moveDirection = NextMove(transform.position);
            _animator.SetFloat("Hori", _moveDirection.x);
            _animator.SetFloat("Var", _moveDirection.y);
        }

        //_rig2d.velocity = _moveDirection * _speed;

        switch (_enemySpeed)
        {
            case EnemySpeedState.NOMAL:
                if (_animObj.activeSelf == true)
                {
                    _animObj.SetActive(false);
                }
                transform.position += _moveDirection * _speed * _speedRate * Time.deltaTime;
                break;
            case EnemySpeedState.DOWN:
                if (_animObj.activeSelf == false)
                {
                    _animObj.SetActive(true);
                }
                transform.position += _moveDirection * _speed * _speedRate * Time.deltaTime * _downSpeed;

                _enemySpeed = EnemySpeedState.NOMAL;
                break;
            default:
                if (_animObj.activeSelf == true)
                {
                    _animObj.SetActive(false);
                }
                transform.position += _moveDirection * _speed * _speedRate * Time.deltaTime;
                break;
        }
        transform.position += _moveDirection * _speed * _speedRate * Time.deltaTime;
    }

    private IEnumerator Des()
    {
        yield return null;

        LevelManager.Instance._enemyManager.DestroyEnemy(gameObject.GetComponent<EnemyCon>());

        Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        HP = HP - damage;

        if (HP <= 0)
        {
            LevelManager.Instance.UseCost();
            Instantiate(_desAnim, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
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

    private void OnDestroy()
    {
        LevelManager.Instance.EnemyCountUpdate();
    }
}


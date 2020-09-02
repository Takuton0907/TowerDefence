using UnityEngine;
public class Enemy : MonoBehaviour 
{ 
    WayPoint wayPoint;
    private Vector3 _move = Vector3.zero;
    private int waypointNum = 0;
    public int HP = 10;
    [SerializeField] float _speed = 1;
    [SerializeField] int _index = 0;

    // Start is called before the first frame update
    void Start()
    {
        wayPoint = GameObject.FindGameObjectWithTag("WayPoint").GetComponent<WayPoint>();
        _move = EnemyMove(_index);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _move * _speed;
        if ((wayPoint._waypointList[waypointNum].List[_index].position - transform.position).magnitude <= 5f)
        {
            _index++;
            if (_index < wayPoint._waypointList[waypointNum].List.Count)
            {
                _move = EnemyMove(_index);
            }
            else
            {
                Debug.Log("deth");
                Destroy(gameObject);
            }
        }

        if (HP <= 0)
        {
            Debug.Log("Deth");
            Destroy(gameObject);
        }
    }

    private Vector3 EnemyMove(int index)
    {
        Vector3 movement = wayPoint._waypointList[waypointNum].List[index].position - transform.position;
        movement.z = 0;
        return movement.normalized;
    }

    public void Damage(int damage)
    {
        HP = HP - damage;
        Debug.Log(HP);
    }
}
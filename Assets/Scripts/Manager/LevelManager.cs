using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public MapDate _mapDate;
    public EnemyManager _enemyManager;
    [SerializeField] Tilemap map;

    private new void Awake()
    {
        _mapDate.MapDateReset();
    }

    private void Update()
    {
        //クリックした位置のタイルのPositionの取得
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit2d.collider != null)
            {
                Vector3Int posi = Vector3Int.RoundToInt(ray.origin);
                posi.z = 0;
                Debug.Log(posi);
            }
        }
    }

    // 指定したTiteStatasのオブジェクト取得
    public List<int> GetIndexs(TILE tileStatas)
    {
        List<int> vs = new List<int>();
        for (int i = 0; i < _mapDate.mapDates.Count; i++)
        {
            if (_mapDate.mapDates[i].tileBaseNum == (int)tileStatas)
            {
                vs.Add(i);
            }
        }
        return vs;
    }
    public List<int> GetIndexs(TILE tileStatas, TILE tileStatas2)
    {
        List<int> vs = new List<int>();
        for (int i = 0; i < _mapDate.mapDates.Count; i++)
        {
            int tileBasenum = _mapDate.mapDates[i].tileBaseNum;
            if (tileBasenum == (int)tileStatas || tileBasenum == (int)tileStatas2)
            {
                vs.Add(i);
            }
        }
        return vs;
    }
    public List<int> GetIndexs(TILE tileStatas, TILE tileStatas2, TILE tileStatas3)
    {
        List<int> vs = new List<int>();
        for (int i = 0; i < _mapDate.mapDates.Count; i++)
        {
            int tileBasenum = _mapDate.mapDates[i].tileBaseNum;
            if (tileBasenum == (int)tileStatas || tileBasenum == (int)tileStatas2 || tileBasenum == (int)tileStatas3)
            {
                vs.Add(i);
            }
        }
        return vs;
    }

    //敵が進む道を検索
    public List<MAP_DATE> Sarch()
    {
        TileOpen();

        int dateIndex = _mapDate.GetStartIndex();
        var openList = new List<int>();

        return Aster(dateIndex, openList);
    }
    //A*の実装
    private List<MAP_DATE> Aster(int index, List<int> list)
    {
        for (int i = 0; i < 1000; i++)
        {
            if (_mapDate.mapDates[index].tileBaseNum == (int)TILE.GOAL)
            {
                Debug.Log("探索終了");
                break;
            }

            //次に動けるTileの取得
            var mapIndexs = _mapDate.GetNextTilesIndex(index);

            //通れる道以外の削除
            foreach (var item in mapIndexs)
            {
                if (_mapDate.mapDates[item].states == TILE_STATAS.Opne)
                {
                    if (_mapDate.mapDates[item].tileBaseNum == (int)TILE.LOAD || _mapDate.mapDates[item].tileBaseNum == (int)TILE.GOAL || _mapDate.mapDates[item].tileBaseNum == (int)TILE.START)
                    {
                        _mapDate.mapDates[item].parentDate = _mapDate.mapDates[index];
                        list.Add(item);
                    }
                }
            }
            //距離の計算
            foreach (var item in mapIndexs)
            {
                _mapDate.mapDates[item].C = _mapDate.GetCost(index, _mapDate.GetGoalIndex());
                _mapDate.mapDates[item].H = _mapDate.GetCost(_mapDate.GetStartIndex(), index);
                _mapDate.mapDates[item].S = _mapDate.mapDates[item].C + _mapDate.mapDates[item].H;
                //Debug.Log("C = " + _mapDate.mapDates[item].C + " H = " + _mapDate.mapDates[item].H);
            }
            //スタートした場所は検索済みなのでCloaseに
            _mapDate.mapDates[index].states = TILE_STATAS.Close;

            int minValue = int.MaxValue;

            foreach (var item in list)
            {
                if (_mapDate.mapDates[item].states == TILE_STATAS.Close) continue;
                if (_mapDate.mapDates[item].S < minValue)
                {
                    minValue = _mapDate.mapDates[item].S;
                    index = item;
                }
            }
        }

        var vs = new List<MAP_DATE>();
        //_mapDate.mapDates[index].GetParh(vs);
        MAP_DATE date = _mapDate.mapDates[index];
        while (date.tileBaseNum != (int)TILE.START)
        {
            vs.Add(date);
            date = date.parentDate;
        }

        return vs;
    }
    //すべてのタイルのSATASをOpenにする
    private void TileOpen()
    {
        foreach (var item in _mapDate.mapDates)
        {
            item.states = TILE_STATAS.Opne;
        }
    }
}

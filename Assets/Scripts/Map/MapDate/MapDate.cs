using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "MyGame/Create ParameterTable", fileName = "ParameterTable")]
public class MapDate : ScriptableObject
{
    public TileBase overTile;
    public TileBase load;
    public TileBase[] wall = new TileBase[2];
    public TileBase setTowet;
    public TileBase start;
    public TileBase goal;
    public List<MAP_DATE> mapDates = new List<MAP_DATE>();
    public Material overTileMaterial;
    public Vector2 mapSize;

    //マップデータの初期化
    public void MapDateReset()
    {
        foreach (var item in mapDates)
        {
            item.states = TILE_STATAS.Opne;
            item.C = 0;
            item.H = 0;
            item.S = 0;
            item.parentDate = null;
            item.tower = false;
        }
    }

    //マップデータのゴールのポジション獲得
    public Vector3 GetGoal()
    {
        foreach (var item in mapDates)
        {
            if (item.tileBaseNum == (int)TILE.GOAL)
            {
                return item.posi;
            }   
        }
        return Vector3.zero;
    }

    //マップデータのゴールのIndex獲得
    public int GetGoalIndex()
    {
        for (int i = 0; i < mapDates.Count; i++)
        {
            if (mapDates[i].tileBaseNum == (int)TILE.GOAL)
            {
                return i;
            }
        }
        return 1000;
    }

    //敵のスタート位置の取得
    public List<Vector3> GetStart()
    {
        List<Vector3> startPositions = new List<Vector3>();
        foreach (var item in mapDates)
        {
            if (item.tileBaseNum == (int)TILE.START)
            {
                startPositions.Add(item.posi);
            }
        }
        return startPositions;
    }
    
    //マップデータのスタートのIndex獲得
    public int GetStartIndex()
    {
        for (int i = 0; i < mapDates.Count; i++)
        {
            if (mapDates[i].tileBaseNum == (int)TILE.START)
            {
                return i;
            }
        }
        return 1000;
    }

    //敵のゴール位置の取得
    public List<MAP_DATE> GetMapDate()
    {
        return mapDates;
    }

    //現在openなTileの取得
    public int GetOpenCout()
    {
        int count = 0;
        foreach (var item in mapDates)
        {
            if (item.states == 0)
            {
                count++;
            } 
        }

        return count;
    }

    //現在いる位置から上下左右のMapDateを取得
    public List<MAP_DATE> GetNextTiles(int StartMapDateIndex)
    {
        List<MAP_DATE> dates = new List<MAP_DATE>();

        if (StartMapDateIndex - (int)mapSize.x > 0)
        {
            //上
            dates.Add(mapDates[StartMapDateIndex - (int)mapSize.x]);
        }
        if (StartMapDateIndex + (int)mapSize.x  < mapDates.Count)
        {
            //下
            dates.Add(mapDates[StartMapDateIndex + (int)mapSize.x]);
        }
        if (StartMapDateIndex % mapSize.x - 1 > 0)
        {
            //左
            dates.Add(mapDates[StartMapDateIndex - 1]);
        }
        if (StartMapDateIndex % mapSize.x - 1 < mapSize.x)
        {
            //右
            dates.Add(mapDates[StartMapDateIndex + 1]);
        }

        return dates;
    }

    //次に進めるTileのIndexの取得
    public List<int> GetNextTilesIndex(int StartMapDateIndex)
    {
        List<int> dates = new List<int>();

        if (StartMapDateIndex - (int)mapSize.y > 0)
        {
            //左
            dates.Add(StartMapDateIndex - (int)mapSize.y);
        }
        if (StartMapDateIndex + (int)mapSize.y < mapDates.Count)
        {
            //右
            dates.Add(StartMapDateIndex + (int)mapSize.y);
        }
        if (StartMapDateIndex % mapSize.y - 1 > 0)
        {
            //上
            dates.Add(StartMapDateIndex - 1);
        }
        if (StartMapDateIndex % mapSize.y - 1 < mapSize.y)
        {
            //下
            dates.Add(StartMapDateIndex + 1);
        }

        return dates;
    }

    public int GetCost(int startIndex, int goalIndex)
    {
        int xDistance = startIndex % (int)mapSize.y - goalIndex % (int)mapSize.y;
        int yDistance = startIndex % (int)mapSize.x - goalIndex % (int)mapSize.x;
        xDistance = Mathf.Abs(xDistance);
        yDistance = Mathf.Abs(yDistance);
        return xDistance + yDistance;
    }
}

[Serializable]
public class MAP_DATE 
{
    public Vector3Int posi;                         //タイルのPosition
    public int tileBaseNum;                         //このタイルがLoadなどかどうかの判定用
    public TILE_STATAS states = TILE_STATAS.Opne;   //このタイルがOpneかの判定
    public int C;                                   //推定コスト
    public int H;                                   //スタート位置からの移動コスト
    public int S;                                   //合計コスト
    public MAP_DATE parentDate;                     //移動してきた経路
    public bool tower = false;                      //towerが設置してあるか
}
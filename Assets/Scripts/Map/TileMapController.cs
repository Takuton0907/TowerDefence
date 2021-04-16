using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TILE
{
    WALL = 0,
    LOAD = 1,
    SET_TOWER = 2,
    START = 3,
    GOAL = 4,
}

public enum TILE_STATAS 
{ 
    Opne,
    Close,
}

/// <summary> タイルマップ設置などをします </summary>
public static class TileMapController
{
    /// <summary> 背景の作成 </summary>
    public static void CreateBackGround(ref Tilemap tilemap, MapDateObject mapDate)
    {
        tilemap.ClearAllTiles();

        Vector2 size = new Vector2(70, 50);
        for (int x = -(int)size.x / 2; x < (int)size.x / 2; ++x)
        {
            for (int y = (int)size.y /2; y >= -(int)size.y / 2; --y)
            {
                Vector3Int posi = new Vector3Int(x, y, 0);
                if (Probability(50))
                {
                    tilemap.SetTile(posi, mapDate.wall[0]);
                }
                else
                {
                    tilemap.SetTile(posi, mapDate.wall[1]);
                }
            }
        }
    }
    /// <summary> マップのタイルを設置します </summary>
    public static void SetMap(ref Tilemap tilemap, MapDateObject mapDate)
    {
        tilemap.ClearAllTiles();        
        
        for (int i = 0; i < mapDate.mapDates.Count; i++)
        {
            if (mapDate.mapDates[i].tileBaseNum == (int)TILE.WALL)
            {
                if (Probability(50))
                {
                    tilemap.SetTile(mapDate.mapDates[i].posi, mapDate.wall[0]);
                }
                else
                {
                    tilemap.SetTile(mapDate.mapDates[i].posi, mapDate.wall[1]);
                }
            }
            else if (mapDate.mapDates[i].tileBaseNum == (int)TILE.LOAD)
            {
                tilemap.SetTile(mapDate.mapDates[i].posi, mapDate.load);
            }
            else if (mapDate.mapDates[i].tileBaseNum == (int)TILE.SET_TOWER)
            {
                tilemap.SetTile(mapDate.mapDates[i].posi, mapDate.setTowet);
            }
            else if (mapDate.mapDates[i].tileBaseNum == (int)TILE.START)
            {
                tilemap.SetTile(mapDate.mapDates[i].posi, mapDate.start);
            }
            else if (mapDate.mapDates[i].tileBaseNum == (int)TILE.GOAL)
            {
                tilemap.SetTile(mapDate.mapDates[i].posi, mapDate.goal);
            }
        }
    }
    /// <summary> タワーを置ける位置をセットします </summary>
    public static void SetToerMap(ref Tilemap tilemap, MapDateObject mapDate, List<int> indexs)
    {
        tilemap.ClearAllTiles();        

        foreach (var item in indexs)
        {
            //Debug.Log(mapDate.mapDates[item].tower);
            if (mapDate.mapDates[item].tower == false)
            {
                tilemap.SetTile(mapDate.mapDates[item].posi, mapDate.overTile);
            }
        }
    }

    /// <summary>
    /// 確率判定
    /// </summary>
    /// <param name="fPercent">確率 (0~100)</param>
    /// <returns>当選結果 [true]当選</returns>
    public static bool Probability(float fPercent)
    {
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;

        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            return true;
        }
        else if (fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
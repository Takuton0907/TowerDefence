using System.Collections.Generic;
using System.Text;
using UnityEditor.Tilemaps;
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

public static class TileMapCon
{
    public static void OutputPosition(Tilemap tilemap)
    {
        var builder = new StringBuilder();
        var bound = tilemap.cellBounds;
        builder.Append("\n");
        for (int y = bound.max.y - 1; y >= bound.min.y; --y)
        {
            for (int x = bound.min.x; x < bound.max.x; ++x)
            {
                builder.Append(tilemap.HasTile(new Vector3Int(x, y, 0)) ? "■" : "□");
                Debug.Log(tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)));
            }
            builder.Append("\n");
        }
        Debug.Log(builder.ToString());
    }

    public static List<Vector3> GetTilePositions(Tilemap tilemap)
    {
        List<Vector3> transforms = new List<Vector3>();

        var bound = tilemap.cellBounds;
        for (int x = bound.min.x; x < bound.max.x; ++x)
        {
            for (int y = bound.max.y - 1; y >= bound.min.y; --y)
            {
                if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    transforms.Add(tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)));
                }
            }
        }
        return transforms;
    }

    //public static List<Vector3> GetNextPositions(Tilemap tilemap, Vector3 nowPosition)
    //{
    //    List<Vector3> transforms = new List<Vector3>();

    //    var bound = tilemap.cellBounds;
    //    for (int x = bound.min.x; x < bound.max.x; ++x)
    //    {
    //        for (int y = bound.max.y - 1; y >= bound.min.y; --y)
    //        {
    //            if (tilemap.HasTile(new Vector3Int(x, y, 0)))
    //            {
    //                transforms.Add(tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)));
    //            }
    //        }
    //    }
    //    return transforms;
    //}

    public static Vector3[,] SetMap(Tilemap tilemap, Vector2 mapSize, TileBase load, TileBase wall)
    {
        Vector3[,] transforms = new Vector3[(int)mapSize.x, (int)mapSize.y];

        for (int y = 0; y < mapSize.y; ++y)
        {
            for (int x = 0; x < mapSize.x; ++x)
            {
                Vector3Int posi = new Vector3Int(x - (int)mapSize.x / 2, y - (int)mapSize.y / 2, 0);
                tilemap.SetTile(posi, load);
                transforms[x, y] = posi;
            }
        }
        return transforms;
    }

    public static void SetMap(ref Tilemap tilemap, MapDate mapDate)
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

    public static void SetToerMap(ref Tilemap tilemap, MapDate mapDate, List<int> indexs)
    {
        tilemap.ClearAllTiles();        



        foreach (var item in indexs)
        {
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
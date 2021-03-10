﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDateCreator : EditorWindow
{
    //MenuItemを付ける事で上部メニューに項目を追加
    [MenuItem("Create/MapDate")]
    [System.Obsolete]
    private static void CreateMapDate()
    {
        MapDateObject mapDate = ScriptableObject.CreateInstance<MapDateObject>();
        //プロジェクトウィンドが選択したものを取得
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

        Debug.Log(selectedAsset[0].name);

        //textを名前から取得
        string date = LoadText.LoadTextDate(selectedAsset[0].name);
        Debug.Log(date);
        string[,] mapStatas = LoadText.SetTexts(date);

        mapDate.mapSize = new Vector2(mapStatas.GetLength(1), mapStatas.GetLength(0));

        for (int x = 0; x < mapStatas.GetLength(1); x++)
        {
            for (int y = 0; y < mapStatas.GetLength(0); y++)
            {
                MAP_C_DATE mAP_DATE = new MAP_C_DATE();
                mAP_DATE.posi = new Vector3Int(x - mapStatas.GetLength(1) / 2, - 1 + (-(y - mapStatas.GetLength(0) / 2)), 0);
                mAP_DATE.tileBaseNum = int.Parse(mapStatas[y, x]);
                mapDate.mapDates.Add(mAP_DATE);
            }
        }

        //スクリタブルObjectとしてファイル書き出し
        AssetDatabase.CreateAsset(mapDate, "Assets/"+ selectedAsset[0].name +".asset");
    }

    [MenuItem("Create/MapPrefab")]
    [System.Obsolete]
    private static void CreatePrefab() 
    {
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        MapDateObject date = (MapDateObject)selectedAsset[0];
        //GameObjectの作成
        GameObject gameObject = new GameObject(selectedAsset[0].name);
        gameObject.AddComponent<Grid>();

        //Mapの作成
        GameObject tilemapObj = new GameObject("Maps");
        tilemapObj.AddComponent<DropArea>();
        tilemapObj.AddComponent<CanvasGroup>();
        TilemapRenderer renderer = tilemapObj.AddComponent<TilemapRenderer>();
        Tilemap tilemap = tilemapObj.GetComponent<Tilemap>();
        TileMapCon.SetMap(ref tilemap, date);
        tilemapObj.transform.parent = gameObject.transform;
        tilemapObj.tag = "Map";

        //キャラをDrag中に見せるタイルの作成
        GameObject overTileObj = new GameObject("OverTile");
        overTileObj.transform.position = new Vector3(0, 0, -1);
        renderer = overTileObj.AddComponent<TilemapRenderer>();
        renderer.material = date.overTileMaterial;
        overTileObj.transform.parent = tilemapObj.transform;
        overTileObj.tag = "OverTile";

        //最背面に出すMapの作成
        GameObject backGroundTilemap = new GameObject("BackGround");
        backGroundTilemap.transform.SetParent(gameObject.transform);
        backGroundTilemap.transform.position += new Vector3(0 ,0 ,-1);
        renderer = backGroundTilemap.AddComponent<TilemapRenderer>();
        tilemap = backGroundTilemap.GetComponent<Tilemap>();
        TileMapCon.CreateBackGround(ref tilemap, date);

        //マップをprefabu化して生成
        PrefabUtility.CreatePrefab("Assets/" + selectedAsset[0].name + ".prefab", gameObject);
        DestroyImmediate(gameObject);
    }

    public static void CreatePrefab(MapDateObject date, string path)
    {
        //GameObjectの作成
        GameObject gameObject = new GameObject(date.name);
        gameObject.AddComponent<Grid>();

        //Mapの作成
        GameObject tilemapObj = new GameObject("Maps");
        tilemapObj.AddComponent<DropArea>();
        tilemapObj.AddComponent<CanvasGroup>();
        TilemapRenderer renderer = tilemapObj.AddComponent<TilemapRenderer>();
        Tilemap tilemap = tilemapObj.GetComponent<Tilemap>();
        TileMapCon.SetMap(ref tilemap, date);
        tilemapObj.transform.parent = gameObject.transform;
        tilemapObj.tag = "Map";

        //キャラをDrag中に見せるタイルの作成
        GameObject overTileObj = new GameObject("OverTile");
        overTileObj.transform.position = new Vector3(0, 0, -1);
        renderer = overTileObj.AddComponent<TilemapRenderer>();
        renderer.material = date.overTileMaterial;
        overTileObj.transform.parent = tilemapObj.transform;
        overTileObj.tag = "OverTile";

        //最背面に出すMapの作成
        GameObject backGroundTilemap = new GameObject("BackGround");
        backGroundTilemap.transform.SetParent(gameObject.transform);
        renderer = backGroundTilemap.AddComponent<TilemapRenderer>();
        tilemap = backGroundTilemap.GetComponent<Tilemap>();
        TileMapCon.CreateBackGround(ref tilemap, date);

        //マップをprefabu化して生成
        PrefabUtility.CreatePrefab(path + date.name + ".prefab", gameObject);
        //AssetDatabase.CreateAsset(gameObject, path);
        DestroyImmediate(gameObject);
    }

    public static MapDateObject CreateMapDate(TextAsset text, string path)
    {
        MapDateObject mapDate = ScriptableObject.CreateInstance<MapDateObject>();
        //textを名前から取得
        string date = LoadText.LoadTextDate(text.name);
        Debug.Log(date);
        string[,] mapStatas = LoadText.SetTexts(date);

        mapDate.mapSize = new Vector2(mapStatas.GetLength(1), mapStatas.GetLength(0));

        for (int x = 0; x < mapStatas.GetLength(1); x++)
        {
            for (int y = 0; y < mapStatas.GetLength(0); y++)
            {
                MAP_C_DATE mAP_DATE = new MAP_C_DATE();
                mAP_DATE.posi = new Vector3Int(x - mapStatas.GetLength(1) / 2, -1 + (-(y - mapStatas.GetLength(0) / 2)), 0);
                mAP_DATE.tileBaseNum = int.Parse(mapStatas[y, x]);
                mapDate.mapDates.Add(mAP_DATE);
            }
        }

        //スクリタブルObjectとしてファイル書き出し
        AssetDatabase.CreateAsset(mapDate, path + text.name + ".asset");
        return mapDate;
    }

    public static void CreateStageAssets()
    {

    }
}
#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDateCreator : EditorWindow
{
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
        TileMapController.SetMap(ref tilemap, date);
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
        TileMapController.CreateBackGround(ref tilemap, date);

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
                GRID_DATA mAP_DATE = new GRID_DATA();
                mAP_DATE.posi = new Vector3Int(x - mapStatas.GetLength(1) / 2, -1 + (-(y - mapStatas.GetLength(0) / 2)), 0);
                mAP_DATE.tileBaseNum = int.Parse(mapStatas[y, x]);
                mapDate.mapDates.Add(mAP_DATE);
            }
        }

        //スクリタブルObjectとしてファイル書き出し
        AssetDatabase.CreateAsset(mapDate, path + text.name + ".asset");
        return mapDate;
    }
}
#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDataCreator : EditorWindow
{
    private static void CreatePrefab(MapDataObject date, string path)
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

    private static MapDataObject CreateMapData(TextAsset text, string path)
    {
        MapDataObject mapDate = ScriptableObject.CreateInstance<MapDataObject>();
        //textを名前から取得
        string date = LoadText.LoadTextData(text.name);
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
                mapDate.mapDatas.Add(mAP_DATE);
            }
        }

        //スクリタブルObjectとしてファイル書き出し
        AssetDatabase.CreateAsset(mapDate, path + text.name + ".asset");
        return mapDate;
    }

    public static void MapCreate(MapData mapData)
    {
        string[] names = AssetDatabase.FindAssets("t:Folder", new[] { "Assets/Resources/Stages" });

        string folderPath = AssetDatabase.CreateFolder("Assets/Resources/Stages", names.Length.ToString("00") + mapData.mapData.name);

        string path = AssetDatabase.GUIDToAssetPath(folderPath);

        path += "/";

        MapDataObject date = CreateMapData(mapData.mapData, path);

        date.load = mapData.load;
        date.wall = mapData.wall.ToArray();
        date.setTowet = mapData.setTower;
        date.overTile = mapData.setTower;
        date.start = mapData.start;
        date.goal = mapData.goal;

        date.overTileMaterial = mapData.overTileMaterial;

        Debug.Log(date.load);

        CreatePrefab(date, path);

        AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(mapData.enemyData), path + mapData.enemyData.name + ".csv");

        Debug.Log($"以下のフォルダに作成し\n{mapData.enemyData.name}も移動しました\n{path}");
    }
}
#endif

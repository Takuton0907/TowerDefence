using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;
using System.Linq;
using UnityEditor.UIElements;

public class StageCreateEditorWindow : EditorWindow
{
    MapCreateGraphView graphView;

    [MenuItem("Window/StageEditor")]
    public static void Open()
    {
        GetWindow<StageCreateEditorWindow>(ObjectNames.NicifyVariableName(nameof(StageCreateEditorWindow)));
    }

    void OnEnable()
    {
        graphView = new MapCreateGraphView(this);
        rootVisualElement.Add(graphView);
        rootVisualElement.style.backgroundColor = Color.gray;

        rootVisualElement.Add(new RunElement("Save", new Color(0.29f, 0.29f, 0.29f), new Vector2(0, 10), OnClickRun));
    }
    //saveの実行
    private void OnClickRun()
    {
        MapNode mapNode = graphView._mapNode;
        var inputChilds = mapNode.inputContainer.contentContainer.Children();

        MapData mapData = new MapData();
        DataType dataType = DataType.GameRoot;

        foreach (var input in mapNode._inputPorts)
        {
            //extensionContainerに設定してある値の参照の取得
            var extensionChildren = input.connections.Select(connect => connect.output?.node.extensionContainer.Children());

            if (extensionChildren.Count() <= 0)
            {
                Debug.LogError($"{input.portName}の値が設定されていないです");
                return;
            }

            foreach (var child in extensionChildren)
            {
                foreach (var item in child)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    //データの格納
                    switch (dataType)
                    {
                        case DataType.GameRoot:
                            if (item is ObjectField rootField)
                            {
                                Debug.Log(rootField.value);
                                mapData._gameRoot = rootField.value as GameObject;
                            }
                            break;
                        case DataType.MapDataText:
                            if (item is TextAssetField mapTextField)
                            {
                                Debug.Log(mapTextField.value);
                                mapData._mapData = mapTextField.value as TextAsset;
                            }
                            break;
                        case DataType.LoadTile:
                            if (item is TileField loadField)
                            {
                                Debug.Log(loadField.value);
                                mapData._load = loadField.value as TileBase;
                            }
                            break;
                        case DataType.WallTile:
                            if (item is TileField wallField)
                            {
                                Debug.Log(wallField.value);
                                if (wallField.value is TileBase tile)
                                {
                                    mapData._wall.Add(tile);
                                }
                            }
                            break;
                        case DataType.SetTowerTile:
                            if (item is TileField towerField)
                            {
                                Debug.Log(towerField.value);
                                mapData._setTower = towerField.value as TileBase;
                            }
                            break;
                        case DataType.StartTile:
                            if (item is TileField startField)
                            {
                                Debug.Log(startField.value);
                                mapData._start = startField.value as TileBase;
                            }
                            break;
                        case DataType.GoalTile:
                            if (item is TileField goalField)
                            {
                                Debug.Log(goalField.value);
                                mapData._goal = goalField.value as TileBase;
                            }
                            break;
                        case DataType.Material:
                            if (item is MaterialField materialField)
                            {
                                Debug.Log(materialField.value);
                                mapData._overTileMaterial = materialField.value as Material;
                            }
                            break;
                        case DataType.EnemyDataText:
                            if (item is TextAssetField enemyTextField)
                            {
                                Debug.Log(enemyTextField.value);
                                mapData._enemyData = enemyTextField.value as TextAsset;
                            }
                            break;
                    }
                }
                dataType += 1;
            }
        }

        string[] names = AssetDatabase.FindAssets("t:Folder", new[] { "Assets/Resources/Stages" });

        string folderPath = AssetDatabase.CreateFolder("Assets/Resources/Stages", names.Length.ToString("00") + mapData._mapData.name);

        string path = AssetDatabase.GUIDToAssetPath(folderPath);

        path += "/";

        MapDataObject date = MapDataCreator.CreateMapData(mapData._mapData, path);

        date.load = mapData._load;
        date.wall = mapData._wall.ToArray();
        date.setTowet = mapData._setTower;
        date.overTile = mapData._setTower;
        date.start = mapData._start;
        date.goal = mapData._goal;

        date.overTileMaterial = mapData._overTileMaterial;

        Debug.Log(date.load);

        MapDataCreator.CreatePrefab(date, path);

        AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(mapData._enemyData), path + mapData._enemyData.name + ".csv");

        Debug.Log($"以下のフォルダに作成し\n{mapData._enemyData.name}も移動しました\n{path}");
    }
}


public class RunElement : VisualElement
{
    public RunElement(string name, Color color, Vector2 pos, Action clickAction)
    {
        style.backgroundColor = new StyleColor(color);
        style.position = Position.Absolute;
        style.height = 50;
        style.width = 100;
        transform.position = pos;

        Add(new Label(name));
        Button runBytton = new Button();
        runBytton.text = "Save";
        runBytton.clickable.clicked += clickAction;
        Add(runBytton);
    }
}

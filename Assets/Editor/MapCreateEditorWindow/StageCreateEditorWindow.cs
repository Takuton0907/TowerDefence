using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;
using System.Linq;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class StageCreateEditorWindow : EditorWindow
{
    /// <summary> 作成したMapCreateGraphViewの参照 </summary>
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

        var mapData = MapDataSet(mapNode);

        if (mapData == null)
        {
            Debug.LogError("マップの作成に失敗しました");
            return;
        }
        MapDataCreator.MapCreate(mapData);
    }

    /// <summary> マップを作るのに必要なデータクラスにまとめる </summary>
    private MapData MapDataSet(MapNode mapNode)
    {
        DataType dataType = DataType.GameRoot;
        MapData mapData = new MapData();

        foreach (var input in mapNode._inputPorts)
        {
            //extensionContainerに設定してある値の参照の取得
            var extensionChildren = input.connections.Select(connect => connect.output?.node.extensionContainer.Children());

            if (extensionChildren.Count() <= 0)
            {
                Debug.LogError($"{input.portName}の値が設定されていないです");
                return null;
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
                                mapData.gameRoot = rootField.value as GameObject;
                            }
                            break;
                        case DataType.MapDataText:
                            if (item is TextAssetField mapTextField)
                            {
                                Debug.Log(mapTextField.value);
                                mapData.mapData = mapTextField.value as TextAsset;
                            }
                            break;
                        case DataType.LoadTile:
                            if (item is TileField loadField)
                            {
                                Debug.Log(loadField.value);
                                mapData.load = loadField.value as TileBase;
                            }
                            break;
                        case DataType.WallTile:
                            if (item is TileField wallField)
                            {
                                Debug.Log(wallField.value);
                                if (wallField.value is TileBase tile)
                                {
                                    mapData.wall.Add(tile);
                                }
                            }
                            break;
                        case DataType.SetTowerTile:
                            if (item is TileField towerField)
                            {
                                Debug.Log(towerField.value);
                                mapData.setTower = towerField.value as TileBase;
                            }
                            break;
                        case DataType.StartTile:
                            if (item is TileField startField)
                            {
                                Debug.Log(startField.value);
                                mapData.start = startField.value as TileBase;
                            }
                            break;
                        case DataType.GoalTile:
                            if (item is TileField goalField)
                            {
                                Debug.Log(goalField.value);
                                mapData.goal = goalField.value as TileBase;
                            }
                            break;
                        case DataType.Material:
                            if (item is MaterialField materialField)
                            {
                                Debug.Log(materialField.value);
                                mapData.overTileMaterial = materialField.value as Material;
                            }
                            break;
                        case DataType.EnemyDataText:
                            if (item is TextAssetField enemyTextField)
                            {
                                Debug.Log(enemyTextField.value);
                                mapData.enemyData = enemyTextField.value as TextAsset;
                            }
                            break;
                    }
                }
                dataType += 1;
            }
        }
        return mapData;
    }
}
/// <summary> 保存するためのボタンの作成 </summary>
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

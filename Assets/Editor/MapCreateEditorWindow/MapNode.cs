using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Collections.Generic;

/// <summary> マップデータのノード </summary>
public class MapNode : Node
{
    MapData _mapData = new MapData();
    public MapNode()
    {
        title = "Map Data";
        // 入力用のポートを作成
        var inputPortGameRoot = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(GameObject));
        inputPortGameRoot.portName = "GameRoot";
        inputPortGameRoot.userData = _mapData._gameRoot;
        inputContainer.Add(inputPortGameRoot);

        var inputPortMapData = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortMapData.portName = "MapData";
        inputPortMapData.userData = _mapData._mapData;
        inputContainer.Add(inputPortMapData);

        var inputPortLoad = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortLoad.portName = "LoadTile";
        inputPortLoad.userData = _mapData._load;
        inputContainer.Add(inputPortLoad);

        var inputPortWall = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(TileBase[]));
        inputPortWall.portName = "WallTile";
        inputPortWall.userData = _mapData._wall;
        inputContainer.Add(inputPortWall);

        var inputPortSetTower = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortSetTower.portName = "SetTowerTile";
        inputPortSetTower.userData = _mapData._setTower;
        inputContainer.Add(inputPortSetTower);

        var inputPortStart = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortStart.portName = "StartTile";
        inputPortStart.userData = _mapData._start;
        inputContainer.Add(inputPortStart);

        var inputPortGoal = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortGoal.portName = "GoalTile";
        inputPortGoal.userData = _mapData._goal;
        inputContainer.Add(inputPortGoal);

        var inputPortOverMaterial = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortOverMaterial.portName = "OverTileMaterial";
        inputPortOverMaterial.userData = _mapData._overTileMaterial;
        inputContainer.Add(inputPortOverMaterial);

        var inputPortEnemyData = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortEnemyData.portName = "MapData";
        inputPortEnemyData.userData = _mapData._enemyData;
        inputContainer.Add(inputPortEnemyData);
    }
}
/// <summary> tilebaseを処理するノード </summary>
public class TileNode : Node
{
    TileField _tileField = default;
    public TileNode()
    {
        title = "Tile";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(TileBase));
        port.portName = "Value";
        port.userData = _tileField;
        outputContainer.Add(port);

        var tileField = new TileField("Tile", defaultPath: "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_00.asset");
        extensionContainer.Add(tileField);
        _tileField = tileField;
        RefreshExpandedState();
    }

    protected override void OnPortRemoved(Port port)
    {
        base.OnPortRemoved(port);
    }
}
/// <summary> tilebaseの配列を処理するノード </summary>
public class TileArrayNode : Node
{
    List<TileField> _tileBases = new List<TileField>();

    public TileArrayNode()
    {
        title = "TileArray";
        //portの作成
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(TileBase[]));
        port.portName = "Value";
        port.userData = _tileBases;
        outputContainer.Add(port);

        //dataの設定とリストへの追加
        var tileField = new TileField("Tile", defaultPath: "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_00.asset");
        extensionContainer.Add(tileField);
        _tileBases.Add(tileField);
        RefreshExpandedState();

        var box = new Box();

        //Addボタンを作成
        var addbutton = new Button();
        addbutton.clickable.clicked += () => {
            //新たなTileFieldを作成
            tileField = new TileField("Tile", defaultPath: "Assets/AssetStore/Texture/Backyard - Free/Separate Tiles/backyard_00.asset");
            int index = extensionContainer.IndexOf(box);
            extensionContainer.Insert(index, tileField);
            _tileBases.Add(tileField);
        };
        addbutton.text = "Add";

        //removeボタンの作成
        var removeButton = new Button();
        removeButton.clickable.clicked += () =>
        {
            //一番下のデータを削除
            int index = extensionContainer.IndexOf(box) - 1;
            if (index <= 0)
            {
                return;
            }
            extensionContainer.RemoveAt(index);
            _tileBases.RemoveAt(index);
        };
        removeButton.text = "Remove";

        // Boxの子としてボタンを追加
        box.Add(addbutton);
        box.Add(removeButton);
        // ルートの子としてBoxを追加
        extensionContainer.Add(box);
    }
}
public class OutputNode : Node
{
    public OutputNode()
    {
        title = "Output";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        port.portName = "Value";
        inputContainer.Add(port);
    }
}

public class AddNode : Node
{
    public AddNode()
    {
        title = "Add";

        var inputPort1 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float[]));
        inputPort1.portName = "A";
        inputContainer.Add(inputPort1);

        var inputPort2 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        inputPort2.portName = "B";
        inputContainer.Add(inputPort2);

        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        outputPort.portName = "Out";
        outputContainer.Add(outputPort);
    }
}

public class ValueNode : Node
{
    public ValueNode()
    {
        title = "Value";

        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        port.portName = "Value";
        outputContainer.Add(port);

        extensionContainer.Add(new FloatField());
        RefreshExpandedState();
    }
}
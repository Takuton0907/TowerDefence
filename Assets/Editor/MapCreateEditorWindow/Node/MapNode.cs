using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

/// <summary> マップデータのノード </summary>
public class MapNode : Node
{
    public List<Port> _inputPorts = new List<Port>();

    public MapNode()
    {
        title = "Map Data";
        // 入力用のポートを作成

        var inputPortGameRoot = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(GameObject));
        inputPortGameRoot.portName = "GameRoot";
        inputContainer.Add(inputPortGameRoot);
        _inputPorts.Add(inputPortGameRoot);

        var inputPortMapData = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortMapData.portName = "MapData";
        inputContainer.Add(inputPortMapData);
        _inputPorts.Add(inputPortMapData);

        var inputPortLoad = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortLoad.portName = "LoadTile";
        inputContainer.Add(inputPortLoad);
        _inputPorts.Add(inputPortLoad);

        var inputPortWall = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase[]));
        inputPortWall.portName = "WallTile";
        inputContainer.Add(inputPortWall);
        _inputPorts.Add(inputPortWall);

        var inputPortSetTower = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortSetTower.portName = "SetTowerTile";
        inputContainer.Add(inputPortSetTower);
        _inputPorts.Add(inputPortSetTower);

        var inputPortStart = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortStart.portName = "StartTile";
        inputContainer.Add(inputPortStart);
        _inputPorts.Add(inputPortStart);

        var inputPortGoal = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TileBase));
        inputPortGoal.portName = "GoalTile";
        inputContainer.Add(inputPortGoal);
        _inputPorts.Add(inputPortGoal);

        var inputPortOverMaterial = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Material));
        inputPortOverMaterial.portName = "OverTileMaterial";
        inputContainer.Add(inputPortOverMaterial);
        _inputPorts.Add(inputPortOverMaterial);

        var inputPortEnemyData = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(TextAsset));
        inputPortEnemyData.portName = "EnemyData";
        inputContainer.Add(inputPortEnemyData);
        _inputPorts.Add(inputPortEnemyData);
    }
}
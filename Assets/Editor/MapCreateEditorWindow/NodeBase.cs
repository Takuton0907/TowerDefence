using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using static UnityEditor.Experimental.GraphView.Port;
using System;

public class NodeBase : Node
{
    protected List<Port> _outputPorts = new List<Port>();
    protected List<Port> _inputPorts = new List<Port>();

    public NodeBase() : base()
    {

    }
    /// <summary> ポートの作成 </summary>
    public PortBase CreatePort(Orientation orientation, Direction direction, Capacity capacity, Type type)
    {
        var port = PortBase.Create<EdgeBase>(orientation, direction, capacity, type);
        switch (direction)
        {
            case Direction.Input:
                _inputPorts.Add(port);
                break;
            case Direction.Output:
                _outputPorts.Add(port);
                break;
        }
        port.OnStopEdgeDragEvent += Execute;
        return port;
    }

    // 出力した先のノードを編集
    public virtual void Execute()
    {
        Debug.Log("output接続");
        foreach (var outputPort in _outputPorts)
        {
            if (outputPort?.connections != null || outputPort?.connections.Any() != false)
            {
                continue;
            }

            IEnumerable nextNodes = outputPort.connections.Select(connect => connect.input?.node);

            if (nextNodes == null)
            {
                break;
            }

            foreach (var nextNode in nextNodes)
            {
                // 出力側に繋がっているノードに対する処理
                Debug.Log("output接続");
            }
        }

        Debug.Log("input接続");
        foreach (var inputPort in _inputPorts)
        {
            if (inputPort?.connections != null || inputPort?.connections.Any() != false)
            {
                continue;
            }

            var prevNodeParameters = inputPort.connections.Select(connect => connect.output?.source);

            if (prevNodeParameters == null || prevNodeParameters.Any() == false)
            {
                break;
            }

            foreach (var parameter in prevNodeParameters)
            {
                // 各パラメータごとの処理
                Debug.Log("input接続");
            }
        }
    }
}
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
using System.Threading.Tasks;

public class NodeBase : Node
{
    public List<Port> _outputPorts { private set; get; } = new List<Port>();
    public List<Port> _inputPorts { private set; get; } = new List<Port>();

    public delegate void DataActioin<T>(T nextNode);
    public DataActioin<object> InputDataActioin { set; get; }
    public DataActioin<Node> OutputDataActioin { set; get; }

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
        port.OnStopEdgeDragEvent += InputExecute;
        port.OnStopEdgeDragEvent += OutputExecute;
        return port;
    }
    /// <summary> portの削除 </summary>
    /// <param name="port">消すport</param>
    /// <param name="direction">inputかoutputの判断</param>
    /// <returns>portが削除されたかを返します</returns>
    public bool RemovePort(PortBase port, Direction direction)
    {
        bool removed = false;
        switch (direction)
        {
            case Direction.Input:
                removed = _inputPorts.Remove(port);
                break;
            case Direction.Output:
                removed = _outputPorts.Remove(port);
                break;
        }

        return removed;
    }

    private async void InputExecute()
    {
        await Stay();
        //現在inputに存在する値を渡す処理
        Debug.Log($"input接続 {_inputPorts.Count}");
        foreach (var inputPort in _inputPorts)
        {
            var nextNodes = inputPort.connections.Select(connect => connect.output?.source);

            if (nextNodes == null || nextNodes.Any() == false)
            {
                Debug.Log($"{nextNodes}");
                break;
            }

            foreach (var nextNode in nextNodes)
            {
                InputDataActioin?.Invoke(nextNode);
            }
        }
    }
    private async void OutputExecute()
    {
        await Stay();
        //現在outputに存在する値を渡す処理
        Debug.Log($"output接続 {_outputPorts.Count}");
        foreach (var outputPort in _outputPorts)
        {
            var nextNodes = outputPort.connections.Select(connect => connect.input?.node);

            if (nextNodes == null || nextNodes.Any() == false)
            {
                Debug.Log($"{nextNodes}");
                break;
            }

            foreach (var nextNode in nextNodes)
            {
                OutputDataActioin?.Invoke(nextNode);
            }
        }
    }

    private async Task Stay()
    {
        await Task.Delay(10);
    }
}
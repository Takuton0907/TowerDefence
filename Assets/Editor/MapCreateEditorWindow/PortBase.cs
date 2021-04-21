using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using System.Linq;
using System;

public class PortBase : Port
{
    public PortBase(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
    {

    }

    public static new PortBase Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
    {
        PortBase portBase = new PortBase(orientation, direction, capacity, type);
        var edgeConnector = new EdgeConnector<TEdge>(new EdgeConnectorListener());
        portBase.m_EdgeConnector = edgeConnector;
        portBase.m_EdgeConnector.edgeDragHelper.draggedPort = portBase;
        
        return portBase;
    }

    public override void OnStartEdgeDragging()
    {
        Debug.Log("OnStart");
        base.OnStartEdgeDragging();
    }
    public override void OnStopEdgeDragging()
    {
        Debug.Log("OnStop");
        base.OnStopEdgeDragging();
    }
}

public class EdgeConnectorListener : IEdgeConnectorListener
{
    public void OnDrop(GraphView graphView, Edge edge)
    {
        Debug.Log("OnDrop");
    }

    public void OnDropOutsidePort(Edge edge, Vector2 position)
    {
        Debug.Log("OnDropOut");
    }
}
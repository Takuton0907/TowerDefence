using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;

public class PortBase : Port
{
    public delegate void OnStartEdgeDrag();
    event OnStartEdgeDrag _onStartEdgeDragEvent;
    public event OnStartEdgeDrag OnStartEdgeDragEvent { add { _onStartEdgeDragEvent += value; } remove { _onStartEdgeDragEvent -= value; } }
    
    public delegate void OnStopEdgeDrag();
    event OnStopEdgeDrag _onStopEdgeDragEvent;
    public event OnStopEdgeDrag OnStopEdgeDragEvent { add { _onStopEdgeDragEvent += value; } remove { _onStopEdgeDragEvent -= value; } }

    public PortBase(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
    {
      
    }

    public static new PortBase Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type) where TEdge : Edge, new()
    {
        var connectorListener = new DefaultEdgeConnectorListener();
        var port = new PortBase(orientation, direction, capacity, type)
        {
            m_EdgeConnector = new EdgeConnector<TEdge>(connectorListener),
        };
        port.AddManipulator(port.m_EdgeConnector);
        return port;
    }

    public override void OnStartEdgeDragging()
    {
        Debug.Log("OnStart");
        base.OnStartEdgeDragging();
        _onStartEdgeDragEvent?.Invoke();
    }
    public override void OnStopEdgeDragging()
    {
        Debug.Log("OnStop");
        base.OnStopEdgeDragging();
        _onStopEdgeDragEvent?.Invoke();
    }
    private class DefaultEdgeConnectorListener : IEdgeConnectorListener
    {
        private GraphViewChange m_GraphViewChange;
        private List<Edge> m_EdgesToCreate;
        private List<GraphElement> m_EdgesToDelete;

        public DefaultEdgeConnectorListener()
        {
            m_EdgesToCreate = new List<Edge>();
            m_EdgesToDelete = new List<GraphElement>();

            m_GraphViewChange.edgesToCreate = m_EdgesToCreate;
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position) { }
        public void OnDrop(GraphView graphView, Edge edge)
        {
            m_EdgesToCreate.Clear();
            m_EdgesToCreate.Add(edge);

            // We can't just add these edges to delete to the m_GraphViewChange
            // because we want the proper deletion code in GraphView to also
            // be called. Of course, that code (in DeleteElements) also
            // sends a GraphViewChange.
            m_EdgesToDelete.Clear();
            if (edge.input.capacity == Capacity.Single)
                foreach (Edge edgeToDelete in edge.input.connections)
                    if (edgeToDelete != edge)
                        m_EdgesToDelete.Add(edgeToDelete);
            if (edge.output.capacity == Capacity.Single)
                foreach (Edge edgeToDelete in edge.output.connections)
                    if (edgeToDelete != edge)
                        m_EdgesToDelete.Add(edgeToDelete);
            if (m_EdgesToDelete.Count > 0)
                graphView.DeleteElements(m_EdgesToDelete);

            var edgesToCreate = m_EdgesToCreate;
            if (graphView.graphViewChanged != null)
            {
                edgesToCreate = graphView.graphViewChanged(m_GraphViewChange).edgesToCreate;
            }

            foreach (Edge e in edgesToCreate)
            {
                graphView.AddElement(e);
                edge.input.Connect(e);
                edge.output.Connect(e);
            }
        }
    }
}
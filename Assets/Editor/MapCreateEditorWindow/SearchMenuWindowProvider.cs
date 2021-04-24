using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class SearchMenuWindowProvider : ScriptableObject, ISearchWindowProvider
{
    private MapCreateGraphView _graphView;
    private EditorWindow _editorWindow;

    public void Initialize(MapCreateGraphView graphView, EditorWindow editorWindow)
    {
        _graphView = graphView;
        _editorWindow = editorWindow;
    }

    List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
    {
        var entries = new List<SearchTreeEntry>();
        entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

        // Exampleというグループを追加
        entries.Add(new SearchTreeGroupEntry(new GUIContent("Tile")) { level = 1 });

        // Exampleグループの下に各ノードを作るためのメニューを追加
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(TileNode))) { level = 2, userData = typeof(TileNode) });
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(TileArrayNode))) { level = 2, userData = typeof(TileArrayNode) });
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(MaterialNode))) { level = 2, userData = typeof(MaterialNode) });
        entries.Add(new SearchTreeEntry(new GUIContent(nameof(TextAssetNode))) { level = 2, userData = typeof(TextAssetNode) });
        
        return entries;
    }

    bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        var type = searchTreeEntry.userData as Type;
        var node = Activator.CreateInstance(type) as Node;

        // マウスの位置にノードを追加
        var worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position);
        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
        node.SetPosition(new Rect(localMousePosition, new Vector2(100, 100)));

        _graphView.AddElement(node);
        return true;
    }
}
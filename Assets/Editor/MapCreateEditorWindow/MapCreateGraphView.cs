using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapCreateGraphView : GraphView
{
    /// <summary> データを作るのに必要なMapNodeの参照 </summary>
    public MapNode _mapNode { private set; get; }

    public MapCreateGraphView(EditorWindow editorWindow) : base()
    {
        _mapNode = new MapNode();
        AddElement(new GameObjectNode());
        AddElement(new TileArrayNode());
        AddElement(new TextAssetNode());
        AddElement(new TextAssetNode());
        AddElement(new MaterialNode());

        for (int i = 0; i < 4; i++)
        {
            AddElement(new TileNode());
        }

        // ノードを追加
        AddElement(_mapNode);

        // 親のサイズに合わせてGraphViewのサイズを設定
        this.StretchToParentSize();

        // MMBスクロールでズームインアウトができるように
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        // MMBドラッグで描画範囲を動かせるように
        this.AddManipulator(new ContentDragger());
        // LMBドラッグで選択した要素を動かせるように
        this.AddManipulator(new SelectionDragger());
        // LMBドラッグで範囲選択ができるように
        this.AddManipulator(new RectangleSelector());

        // 右クリックメニューを追加
        var menuWindowProvider = ScriptableObject.CreateInstance<SearchMenuWindowProvider>();
        menuWindowProvider.Initialize(this, editorWindow);
        nodeCreationRequest += context =>
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
        };
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        compatiblePorts.AddRange(ports.ToList().Where(port =>
        {
            // 同じノードには繋げない
            if (startPort.node == port.node)
                return false;

            // Input同士、Output同士は繋げない
            if (port.direction == startPort.direction)
                return false;

            // ポートの型が一致していない場合は繋げない
            if (port.portType != startPort.portType)
                return false;

            return true;
        }));

        return compatiblePorts;
    }
}

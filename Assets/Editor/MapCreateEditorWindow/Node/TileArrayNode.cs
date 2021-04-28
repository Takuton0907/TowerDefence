using UnityEditor.Experimental.GraphView;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Collections.Generic;
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
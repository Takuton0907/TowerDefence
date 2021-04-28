using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>  TileBaseを管理するField </summary>
public class TileField : ObjectField
{
    public TileField(string label, string tooltip = "", string defaultPath = "") : base()
    {
        objectType = typeof(TileBase);

        base.label = label;
        base.labelElement.style.color = Color.white;
        base.tooltip = tooltip;
        base.value = AssetDatabase.LoadAssetAtPath(defaultPath, typeof(TileBase)) as TileBase;
    }
}
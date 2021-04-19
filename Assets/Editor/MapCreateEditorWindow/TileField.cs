using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TileField : ObjectField
{
    public TileField(string label, string tooltip = "", string defaultPath = "")
    {
        objectType = typeof(TileBase);
        base.label = label;
        base.labelElement.style.color = Color.white;
        base.tooltip = tooltip;
        base.style.backgroundColor = new Color(0.294f, 0.294f, 0.294f);
        base.value = AssetDatabase.LoadAssetAtPath(defaultPath, typeof(TileBase)) as TileBase;
    }
}

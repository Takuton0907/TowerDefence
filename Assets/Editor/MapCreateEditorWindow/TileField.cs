﻿using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileField : BaseField<TileBase>
{
    public TileField(string label, VisualElement visualInput, string tooltip = "", string defaultPath = "") : base(label, visualInput)
    {
        base.label = label;
        base.labelElement.style.color = Color.white;
        base.tooltip = tooltip;
        base.style.backgroundColor = new Color(0.294f, 0.294f, 0.294f);
        base.value = AssetDatabase.LoadAssetAtPath(defaultPath, typeof(TileBase)) as TileBase;
    }
}
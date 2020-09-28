using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectSceneManager : MonoBehaviour
{
    [SerializeField]
    Button _selectButton;

    [SerializeField]
    Transform _parentTrans;

    string assetsPath = "Assets/Resources/Stages";

    private void Awake()
    {
        string[] names = AssetDatabase.FindAssets("t:Folder", new[] { assetsPath });

        foreach (var item in names)
        {
            string name = AssetDatabase.GUIDToAssetPath(item);

            if (name == assetsPath + "/00MapDates") continue;
#if !UNITY_EDITOR
            if (name == assetsPath + "/01testMap")continue;
#endif
            name = name.Substring("Assets/Resources/".Length);

            Button button = Instantiate(_selectButton, _parentTrans);
            button.name = name;

            name = name.Substring("Stages/".Length + 2);
            button.GetComponentInChildren<Text>().text = name;
        }
    }
}

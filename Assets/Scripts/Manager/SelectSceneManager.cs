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

    string assetsPath = "Assets/Scenes/GameScenes";

    private void Awake()
    {
        string[] names = AssetDatabase.FindAssets("t:Scene", new[] { assetsPath });


        foreach (var item in names)
        {
            string name = AssetDatabase.GUIDToAssetPath(item);
            name = name.Substring(assetsPath.Length + 1);
            name = name.Substring(0, name.Length - 6);
            Button button = Instantiate(_selectButton, _parentTrans);
            button.name = name;
            name = name.Substring(2);
            button.GetComponentInChildren<Text>().text = name;
        }
    }


}

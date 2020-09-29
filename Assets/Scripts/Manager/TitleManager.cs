using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] MapDate _titleMapdate;

    [SerializeField] TextAsset _spawnText;

    string[,] _texts;
    // Start is called before the first frame update
    void Start()
    {
        _texts = LoadText.SetTexts(_spawnText.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

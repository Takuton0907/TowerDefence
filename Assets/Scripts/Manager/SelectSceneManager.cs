using UnityEngine;
using UnityEngine.UI;

public class SelectSceneManager : MonoBehaviour
{
    [SerializeField]
    Button _selectButton;

    [SerializeField]
    Transform _parentTrans;

    AudioSource _bgmAudio;

    string assetsPath = "Stages";

    private void Awake()
    {
        _bgmAudio = GetComponent<AudioSource>();

        StartCoroutine(SoundManager.Instance.SetBgmAudio(_bgmAudio.clip));
        
        Object[] names2 = Resources.LoadAll(assetsPath);

        foreach (var item in names2)
        {

            if (item.name == assetsPath + "/00MapDates") continue;
#if !UNITY_EDITOR
            if (name == assetsPath + "/01testMap")continue;
#endif
            Button button = Instantiate(_selectButton, _parentTrans);
            button.name = item.name;

            button.GetComponentInChildren<Text>().text = item.name;
        }
    }
}

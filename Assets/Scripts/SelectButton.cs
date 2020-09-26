using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
    public void OcClickGameStart()
    {
        FadeManager.Instance.LoadScene(gameObject.name, 1.5f);
    }
}

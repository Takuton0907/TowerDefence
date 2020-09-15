using UnityEngine;

public abstract class TowerMonoBehaviur : MonoBehaviour
{
    protected Canvas myCanvas;

    public Transform parentTransform;

    public float _area = 2;

    virtual public void Init()
    {
        myCanvas = GetComponentInChildren<Canvas>();
        myCanvas.gameObject.SetActive(false);
    }

    virtual public void OnClickOpenCanvas()
    {
        myCanvas.gameObject.SetActive(true);
    }
}
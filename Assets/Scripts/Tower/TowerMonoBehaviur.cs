using UnityEngine;

public class TowerMonoBehaviur : MonoBehaviour
{
    private Canvas myCanvas;
    public Transform parentTransform;

    private void Start()
    {
        myCanvas = GetComponentInChildren<Canvas>();
        myCanvas.gameObject.SetActive(false);
    }

    public void OnClickOpenCanvas()
    {
        myCanvas.gameObject.SetActive(true);
    }
}
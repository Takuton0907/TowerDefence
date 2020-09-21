using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerMonoBehaviur : MonoBehaviour
{
    protected Canvas myCanvas;

    protected List<TowerAnimBase> _anims = new List<TowerAnimBase>();

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

    public virtual void RemoveAnim(TowerAnimBase towerAnimBase) { _anims.Remove(towerAnimBase); }
    public virtual void Action(float speed) {}
    public virtual void Action(List<EnemyCon> enemy, float speed) { }
}
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary> タワーを置ける場所の管理 </summary>
public class DropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData data)
    {
        Debug.Log(gameObject.name);

        TowerBase dragObj = data.pointerDrag.GetComponent<TowerBase>();
        if (dragObj != null)
        {
            dragObj.parentTransform = this.transform;
            Debug.Log(gameObject.name + "に" + data.pointerDrag.name + "をドロップ");
        }
    }
}

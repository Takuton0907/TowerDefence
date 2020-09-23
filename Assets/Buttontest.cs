using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buttontest : MonoBehaviour, IPointerClickHandler
{
    

    public void click()
    {
        Debug.Log("Click");
    }

    public void Onclick()
    {
        Debug.Log("ONClick");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem eventSystem = GetComponent<EventSystem>();
        //eventSystem.SetSelectedGameObject(gameObject);
        Debug.Log(eventSystem.currentSelectedGameObject);
    }
}

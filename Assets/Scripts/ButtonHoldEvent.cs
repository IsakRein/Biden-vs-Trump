using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonHoldEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button btn;
    public UnityEvent down;
    public UnityEvent up;

    
    void Start() 
    {
        btn = gameObject.GetComponent<Button>();
    }

    //Do this when the mouse is clicked over the selectable object this script is attached to.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (btn.interactable){
        down.Invoke();

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (btn.interactable){
        up.Invoke();

        }
    }
}

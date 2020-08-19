using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMoveText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button btn;
    public Transform text;
    public float move_down;

    void Start() 
    {
        btn = gameObject.GetComponent<Button>();
    }

    //Do this when the mouse is clicked over the selectable object this script is attached to.
    public void OnPointerDown(PointerEventData eventData)
    {
        text.localPosition -= new Vector3(0, move_down, 0);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.localPosition += new Vector3(0, move_down, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookField : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
{
    private bool isPressed;
    private int pointerID;
    private Vector2 prevPointerPos;
    private Vector2 pointerDist;

    private void Update()
    {
        if(isPressed)
        {
            if(pointerID >= 0 && pointerID<Input.touchCount) 
            {
                pointerDist = Input.GetTouch(pointerID).position - prevPointerPos;
            }
            else
            {
                Vector2 mousePos =  new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                pointerDist = mousePos - prevPointerPos;
                prevPointerPos = mousePos;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pointerID = eventData.pointerId;
        prevPointerPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
    public Vector2 GetPointerDist()
    {
        return pointerDist;
    }
}

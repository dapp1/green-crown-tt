using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Action clicked;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        clicked?.Invoke();
    }
}

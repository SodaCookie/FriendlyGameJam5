using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class TouchDragToMouse : MonoBehaviour, IDragHandler
{
    public float sensitivity = 0.5f;

    private float dX, dY;
    private bool draggedThisFrame;

    public void OnDrag(PointerEventData eventData)
    {
        dX = eventData.delta.x;
        dY = eventData.delta.y;
        draggedThisFrame = true;
    }

    private void Update()
    {
        if (!draggedThisFrame)
        {
            dX = dY = 0;
        }
        CrossPlatformInputManager.SetAxis("Mouse X", sensitivity * dX);
        CrossPlatformInputManager.SetAxis("Mouse Y", sensitivity * dY);
        draggedThisFrame = false;
    }
}

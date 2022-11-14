using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionRule : MonoBehaviour
{
    public virtual bool OnTarget(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftControl) &&
                !Input.GetKey(KeyCode.RightControl))
        {
            SelectableObject.DeselectAll(eventData);
        }
        return true;
    }

    public virtual bool IsSelectable()
    {
        return true;
    }
}

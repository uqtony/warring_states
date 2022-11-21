using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NotifyParentSelection : MonoBehaviour
{
    public void OnSelect(BaseEventData eventData)
    {        
        transform.parent.gameObject.SendMessage("OnSelect", eventData,
            SendMessageOptions.DontRequireReceiver);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.parent.gameObject.SendMessage("OnDeselect", eventData,
            SendMessageOptions.DontRequireReceiver);
    }

    public void OnSelected(BaseEventData eventData)
    {
        transform.parent.gameObject.SendMessage("OnSelect", eventData,
            SendMessageOptions.DontRequireReceiver);
    }

    public void OnDeselected(BaseEventData eventData)
    {
        transform.parent.gameObject.SendMessage("OnDeselect", eventData,
            SendMessageOptions.DontRequireReceiver);
    }
}

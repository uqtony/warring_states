using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AreaSelectionHandler : MonoBehaviour
{
    public void OnSelect(BaseEventData eventData)
    {
        GetComponentInChildren<SelectionMark>().OnSelect(eventData);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        GetComponentInChildren<SelectionMark>().OnDeselect(eventData);
    }
}

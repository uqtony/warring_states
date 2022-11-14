using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableObject : MonoBehaviour, ISelectHandler, IPointerClickHandler
{
    public static HashSet<SelectableObject> allSelectable = new HashSet<SelectableObject>();
    public static HashSet<SelectableObject> currentlySelected = new HashSet<SelectableObject>();

    public GameObject selectedMark;

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectionRule selectionRule = GetComponent<SelectionRule>();
        if (selectionRule != null)
        {
            if (!selectionRule.IsSelectable())
                return;
            if(!selectionRule.OnTarget(eventData))
            {
                return;
            }
        }
        else if (!Input.GetKey(KeyCode.LeftControl) &&
                !Input.GetKey(KeyCode.RightControl))
        {
            DeselectAll(eventData);
        }
        OnSelect(eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        currentlySelected.Add(this);
        notifySelected(eventData);
        updateSelectedMark(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        updateSelectedMark(false);
        notifyDeselected(eventData);
    }

    public static void DeselectAll(BaseEventData eventData)
    {
        foreach(SelectableObject selectableObject in currentlySelected)
        {
            selectableObject.OnDeselect(eventData);
        }
        currentlySelected.Clear();
    }

    public void notifySelected(BaseEventData eventData)
    {
        SendMessage("OnSelected", eventData, SendMessageOptions.DontRequireReceiver);
    }

    public void notifyDeselected(BaseEventData eventData)
    {
        SendMessage("OnDeselected", eventData, SendMessageOptions.DontRequireReceiver);
    }

    void updateSelectedMark(bool isSelected)
    {
        if (selectedMark == null)
            return;
        selectedMark.SetActive(isSelected);
    }

    void Awake()
    {
        allSelectable.Add(this);
    }
}

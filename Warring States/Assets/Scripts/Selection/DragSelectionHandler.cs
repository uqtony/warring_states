using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragSelectionHandler : MonoBehaviour, IBeginDragHandler, IDragHandler,
    IEndDragHandler, IPointerClickHandler
{
    public List<GameObject> pointClickIgnoreObjects = new List<GameObject>();
    [SerializeField]
    Image selectionBoxImage;

    Vector2 startPosition;
    Rect selectionRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
            SelectableObject.DeselectAll(new BaseEventData(EventSystem.current));
        selectionBoxImage.gameObject.SetActive(true);
        startPosition = eventData.position;
        selectionRect = new Rect();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.position.x < startPosition.x)
        {
            selectionRect.xMin = eventData.position.x;
            selectionRect.xMax = startPosition.x;
        }
        else
        {
            selectionRect.xMin = startPosition.x;
            selectionRect.xMax = eventData.position.x;
        }

        if (eventData.position.y < startPosition.y)
        {
            selectionRect.yMin = eventData.position.y;
            selectionRect.yMax = startPosition.y;
        }
        else
        {
            selectionRect.yMin = startPosition.y;
            selectionRect.yMax = eventData.position.y;
        }

        selectionBoxImage.rectTransform.offsetMin = selectionRect.min;
        selectionBoxImage.rectTransform.offsetMax = selectionRect.max;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        selectionBoxImage.gameObject.SetActive(false);
        foreach(SelectableObject selectableObject in SelectableObject.allSelectable)
        {
            if (selectionRect.Contains(Camera.main.WorldToScreenPoint(selectableObject.transform.position)))
            {
                selectableObject.OnSelect(eventData);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        float myDistance = 0;
        foreach(RaycastResult result in results)
        {
            if (result.gameObject == this.gameObject)
            {
                myDistance = result.distance;
                break;
            }
        }// end foreach

        GameObject nextObject = null;
        float maxDistance = Mathf.Infinity;

        string msg = "";
        foreach (RaycastResult result in results)
        {
            msg += "name=" + result.gameObject.name + ", distance=" + result.distance + " >> ";
            if (result.distance > myDistance && result.distance < maxDistance)
            {
                nextObject = result.gameObject;
                maxDistance = result.distance;
            }

        }//end foreach
        Ray castPoint = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit[] hits;
        LayerMask hitLayers = LayerMask.GetMask("Default");
        hits = Physics.RaycastAll(castPoint);
        msg += "___";
        foreach(RaycastHit hit in hits)
        {
            bool shouldIgnore = false;
            foreach (GameObject ignoreObject in pointClickIgnoreObjects)
            {
                if (hit.transform.gameObject == ignoreObject)
                {
                    shouldIgnore = true;
                    break;
                }
            }
            if (shouldIgnore)
                continue;
            msg += "name=" + hit.transform.name + ", distance="+hit.distance+" >> ";
            if (hit.distance > myDistance && hit.distance < maxDistance)
            {
                nextObject = hit.transform.gameObject;
                maxDistance = hit.distance;
            }

        }
        if (nextObject)
        {
            triggerPointClickEvent(nextObject, eventData);
        }
       
    }// end OnPointerClick

    void triggerPointClickEvent(GameObject target, PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerClickHandler>(target, eventData,
                (x, y) => { x.OnPointerClick((PointerEventData)y); });
    }
}

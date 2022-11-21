using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AreaSelectionMark : SelectionMark
{
    public Material selectedMaterial;
    Material originMaterial;
    Material material;
    Renderer _renderer;

    public override void OnSelect(BaseEventData eventData)
    {
        //getMaterial().EnableKeyword("_EMISSION");
        if (selectedMaterial != null)
        {
            originMaterial = GetRenderer().material;
            GetRenderer().material = selectedMaterial;
        }            
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        //getMaterial().DisableKeyword("_EMISSION");
        if (selectedMaterial != null)
        {            
            GetRenderer().material = originMaterial;
        }
    }

    Material getMaterial()
    {
        if (material == null)
        {
            material = GetComponent<Renderer>().material;
        }
        return material;
    }

    Renderer GetRenderer()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        return _renderer;
    }
}
